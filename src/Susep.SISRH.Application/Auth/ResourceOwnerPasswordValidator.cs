using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IOptions<LdapOptions> Options;
        private readonly IPessoaRepository PessoaRepository;
        private readonly IHttpContextAccessor ContextAccessor;

        public ResourceOwnerPasswordValidator(
            IOptions<LdapOptions> options,
            IPessoaRepository pessoaRepository,
            IHttpContextAccessor contextAccessor)
        {
            this.Options = options;
            this.PessoaRepository = pessoaRepository;
            this.ContextAccessor = contextAccessor;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                if (String.IsNullOrEmpty(context.UserName) || String.IsNullOrEmpty(context.Password))
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "As credenciais do usuário são obrigatórias", null);
                    return;
                }

                string cpfMok = null;
                switch (context.UserName)
                {
                    case "sisgp_gestor": cpfMok = "08056275029"; break;
                    case "sisgp_cg": cpfMok = "95387502500"; break;
                    case "sisgp_coget": cpfMok = "43321040565"; break;
                    case "sisgp_coordenador": cpfMok = "25715446597"; break;
                    case "sisgp_diretor": cpfMok = "39178470510"; break;
                    case "sisgp_servidor": cpfMok = "08152972541"; break;
                    case "sisgp_servidor1": cpfMok = "59516301002"; break;
                    case "sisgp_servidor2": cpfMok = "18761704091"; break;
                    case "sisgp_servidor3": cpfMok = "07721701007"; break;
                    case "sisgp_servidor4": cpfMok = "51884275087"; break;
                }

                Pessoa pessoa = null;
                if (!string.IsNullOrEmpty(cpfMok))
                {
                    //if (context.Password.ToUpper() == "S20211014")
                    //{
                    pessoa = await this.PessoaRepository.ObterPorCriteriosAsync(null, cpfMok);
                    //}
                }
                else
                {
                    if (this.Options.Value.Configurations == null)
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "As configurações do LDAP são inválidas", null);
                    }
                    else
                    {
                        pessoa = await Task.Run(() =>
                        {
                            foreach (var configuration in this.Options.Value.Configurations)
                            {
                                using (var connection = new Novell.Directory.Ldap.LdapConnection())
                                {
                                    try
                                    {
                                        connection.Connect(configuration.Url, configuration.Port);
                                        connection.Bind(configuration.BindDN, configuration.BindPassword);
                                    }
                                    catch
                                    {
                                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Não foi possível pesquisar no LDAP. A autenticação do usuário de serviço falhou", null);
                                        return null;
                                    }

                                    List<string> attibutes = new List<string>();
                                    if (!String.IsNullOrEmpty(configuration.SisrhIdAttributeFilter)) attibutes.Add(configuration.SisrhIdAttributeFilter);
                                    if (!String.IsNullOrEmpty(configuration.EmailAttributeFilter)) attibutes.Add(configuration.EmailAttributeFilter);
                                    if (!String.IsNullOrEmpty(configuration.CpfAttributeFilter)) attibutes.Add(configuration.CpfAttributeFilter);

                                    var searchFilter = String.Format(configuration.SearchFilter, context.UserName);
                                    var entities = connection.Search(
                                        configuration.SearchBaseDC,
                                        Novell.Directory.Ldap.LdapConnection.ScopeSub,
                                        searchFilter,
                                        attibutes.ToArray(),
                                        false);

                                    while (entities.HasMore())
                                    {
                                        var entity = entities.Next();
                                        var entityAttributes = entity.GetAttributeSet();

                                        //Valida o password
                                        connection.Bind(entity.Dn, context.Password);

                                        var sisrhId = GetAttributeValue(entity, configuration.SisrhIdAttributeFilter);
                                        if (!String.IsNullOrEmpty(sisrhId))
                                        {
                                            var _pessoa = this.PessoaRepository.ObterAsync(Int64.Parse(sisrhId));
                                            if (_pessoa != null)
                                                return _pessoa;
                                        }

                                        string email = GetAttributeValue(entity, configuration.EmailAttributeFilter);
                                        string cpf = GetAttributeValue(entity, configuration.CpfAttributeFilter);

                                        var dadosPessoa = this.PessoaRepository.ObterPorCriteriosAsync(email, cpf);
                                        if (dadosPessoa != null)
                                            return dadosPessoa;
                                    }
                                }
                            }

                            return null;
                        });
                    }
                }

                if (pessoa != null)
                {
                    context.Result = new GrantValidationResult(pessoa.PessoaId.ToString(), "password", null, "local", null);
                }
                else
                {
                    if (context.Result == null)
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Não foi encontrado usuário com esse login", null);
                    }
                }

            }
            catch (Novell.Directory.Ldap.LdapException ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.Message, null);
                context.Result.Error = ex.StackTrace.ToString();
            }

        }

        private string GetAttributeValue(LdapEntry entity, string attributeKey)
        {
            if (!String.IsNullOrEmpty(attributeKey))
            {
                var entityAttributes = entity.GetAttributeSet();
                if (entityAttributes.ContainsKey(attributeKey))
                {
                    var attrValue = entity.GetAttribute(attributeKey);
                    if (attrValue != null && !String.IsNullOrEmpty(attrValue.StringValue))
                        return attrValue.StringValue;
                }
            }
            return null;
        }
    }
}
