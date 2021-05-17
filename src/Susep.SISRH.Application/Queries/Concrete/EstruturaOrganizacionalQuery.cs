using Dapper;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.RawSql;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Result.Abstractions;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Concrete
{
    /// <summary>
    /// Resolve os controles de acesso aos dados de acordo com o perfil do usuário
    /// </summary>
    public class EstruturaOrganizacionalQuery : IEstruturaOrganizacionalQuery
    {
        private readonly IPessoaQuery PessoaQuery;
        private readonly IUnidadeQuery UnidadeQuery;
        private readonly IDominioQuery DominioQuery;
        private readonly IPlanoTrabalhoQuery PlanoTrabalhoQuery;
        private readonly IPactoTrabalhoQuery PactoTrabalhoQuery;

        public EstruturaOrganizacionalQuery(
            IPessoaQuery pessoaQuery,
            IUnidadeQuery unidadeQuery,
            IDominioQuery dominioQuery,
            IPlanoTrabalhoQuery planoTrabalhoQuery,
            IPactoTrabalhoQuery pactoTrabalhoQuery
        )
        {
            PessoaQuery = pessoaQuery;
            UnidadeQuery = unidadeQuery;
            DominioQuery = dominioQuery;
            PlanoTrabalhoQuery = planoTrabalhoQuery;
            PactoTrabalhoQuery = pactoTrabalhoQuery;
        }

        #region Perfil Pessoa

        public async Task<IApplicationResult<PessoaPerfilViewModel>> ObterPerfilPessoaAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<PessoaPerfilViewModel>();

            var dadosPessoa = await PessoaQuery.ObterPorChaveAsync(request.UsuarioLogadoId);

            result.Result = new PessoaPerfilViewModel()
            {
                PessoaId = dadosPessoa.Result.PessoaId,
                Nome = dadosPessoa.Result.Nome,
                UnidadeId = dadosPessoa.Result.UnidadeId,
                Perfis = new List<PessoaPerfilAcessoViewModel>()
            };

            #region Perfil modo avançado

            var perfilExibirModoAvancado = await DominioQuery.ObterDominioPorClassificacaoAsync(ClassificacaoCatalogoDominioEnum.ModoAvancado);
            if (perfilExibirModoAvancado.Result.Any(g => Int64.Parse(g.Descricao) == request.UsuarioLogadoId))
                result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.ModoAvancado });

            #endregion

            #region Perfis de administração do sistema

            var gestores = await DominioQuery.ObterDominioPorClassificacaoAsync(ClassificacaoCatalogoDominioEnum.GestorSistema);
            if (gestores.Result.Any(g => Int64.Parse(g.Descricao) == request.UsuarioLogadoId))
                result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.Gestor });

            #endregion

            #region Perfis por unidade

            if (dadosPessoa.Result.TipoFuncaoUnidadeId == (int)TipoFuncaoUnidadeEnum.COGEP)
            {
                result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.GestorPessoas });
            }

            if (dadosPessoa.Result.TipoFuncaoUnidadeId == (int)TipoFuncaoUnidadeEnum.COGET)
            {
                result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.GestorIndicadores });
            }

            #endregion

            #region Perfis pelo servidor

            if (dadosPessoa.Result.TipoFuncaoId.HasValue)
            {
                //Se tiver função na unidade dele, tem perfil servidor na unidade acima
                var unidadeChefe = await UnidadeQuery.ObterPorChaveAsync(dadosPessoa.Result.UnidadeId);
                if (unidadeChefe.Result.UnidadeIdPai.HasValue)
                    result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.Servidor, Unidades = new List<long>() { unidadeChefe.Result.UnidadeIdPai.Value } });

                if (dadosPessoa.Result.Chefe.HasValue && dadosPessoa.Result.Chefe.Value)
                {
                    //Obtém as unidades subordinadas à que a pessoa é chefe
                    var unidadesSubordinadas = await UnidadeQuery.ObterSubordinadasAsync(dadosPessoa.Result.UnidadeId);
                    var idsUnidades = unidadesSubordinadas.Result.Select(u => Int64.Parse(u.Id)).ToList();

                    //Adiciona os perfis de chefia
                    result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.ChefeUnidade, Unidades = idsUnidades });

                    if (dadosPessoa.Result.TipoFuncaoId == (int)TipoFuncaoPessoaEnum.ChefeDepartamento ||
                        dadosPessoa.Result.TipoFuncaoId == (int)TipoFuncaoPessoaEnum.CoordenadorGeral)
                    {
                        result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.CoordenadorGeral, Unidades = idsUnidades });
                    }

                    if (dadosPessoa.Result.TipoFuncaoId == (int)TipoFuncaoPessoaEnum.Superintendente ||
                        dadosPessoa.Result.TipoFuncaoId == (int)TipoFuncaoPessoaEnum.Diretor)
                    {
                        result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.Diretor, Unidades = idsUnidades });
                    }
                }
            }
            else
            {
                result.Result.Perfis.Add(new PessoaPerfilAcessoViewModel() { Perfil = (int)PerfilUsuarioEnum.Servidor, Unidades = new List<long>() { dadosPessoa.Result.UnidadeId } });
            }

            #endregion

            return result;
        }

        private async Task<IEnumerable<Int64>> ObterUnidadesPerfilPessoa(UsuarioLogadoRequest request)
        {
            var perfisUsuario = await ObterPerfilPessoaAsync(request);

            var unidades = new List<Int64>() { perfisUsuario.Result.UnidadeId };

            //Se não for gestor do sistema, deve filtrar
            if (!perfisUsuario.Result.Perfis.Any(p => p.Perfil == (int)PerfilUsuarioEnum.Gestor))
            {
                //Obtém as unidades em que o usuário é chefe
                var unidadesChefia = (from perfil in perfisUsuario.Result.Perfis                                      
                                      from unidade in perfil.Unidades
                                      select unidade).Distinct().ToList();

                //Filtra pelas unidades em que é chefe ou o plano é dele
                unidades.AddRange(unidadesChefia);
                return unidades;
            }
            return null;
        }

        #endregion

        #region Métodos protegidos por perfil

        #region Unidades

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterUnidadesAtivasDadosComboAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            //Obtém as unidades
            var dados = await UnidadeQuery.ObterAtivasAsync();

            //Filtra as unidades de acordo com o perfil do usuário
            var unidades = await RestringirUnidadesPerfilUsuario(request, dados.Result, true);

            //Converte de UnidadeViewModel para DadosComboViewModel
            result.Result = unidades.Select(u => new DadosComboViewModel() { Id = u.UnidadeId.ToString(), Descricao = u.SiglaCompleta });

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterUnidadesComPlanoTrabalhoDadosComboAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            //Obtém as unidades
            var dados = await UnidadeQuery.ObterComPlanoTrabalhoAsync();

            //Filtra as unidades de acordo com o perfil do usuário
            var unidades = await RestringirUnidadesPerfilUsuario(request, dados.Result, true);

            //Converte de UnidadeViewModel para DadosComboViewModel
            result.Result = unidades.Select(u => new DadosComboViewModel() { Id = u.UnidadeId.ToString(), Descricao = u.SiglaCompleta });

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterComCatalogoCadastradoComboAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            //Obtém as unidades
            var dados = await UnidadeQuery.ObterComCatalogoCadastradoComboAsync();

            //Filtra as unidades de acordo com o perfil do usuário
            var unidades = await RestringirUnidadesPerfilUsuario(request, dados.Result, false);

            //Converte de UnidadeViewModel para DadosComboViewModel
            result.Result = unidades.Select(u => new DadosComboViewModel() { Id = u.UnidadeId.ToString(), Descricao = u.SiglaCompleta });

            return result;
        }



        private async Task<IEnumerable<UnidadeViewModel>> RestringirUnidadesPerfilUsuario(UsuarioLogadoRequest request, IEnumerable<UnidadeViewModel> items, Boolean incluirUnidadePaiSeChefe)
        {
            var retorno = items.ToList();
            var perfisUsuario = await ObterPerfilPessoaAsync(request);

            //Se não for gestor do sistema, deve filtrar
            if (!perfisUsuario.Result.Perfis.Any(p => p.Perfil == (int)PerfilUsuarioEnum.Gestor))
            {
                //Obtém as unidades em que o usuário é chefe
                var unidadesChefia = (from perfil in perfisUsuario.Result.Perfis
                                      where perfil.Perfil == (int)PerfilUsuarioEnum.ChefeUnidade ||
                                            perfil.Perfil == (int)PerfilUsuarioEnum.CoordenadorGeral ||
                                            perfil.Perfil == (int)PerfilUsuarioEnum.Diretor
                                      from unidade in perfil.Unidades
                                      select unidade).Distinct().ToList();


                //Filtra pelas unidades em que é chefe ou o plano é dele
                retorno = items.Where(p => unidadesChefia.Any(uu => uu == p.UnidadeId) || p.UnidadeId == perfisUsuario.Result.UnidadeId).ToList();

                //Se for para incluir a unidade pai se o usuário for chefe, encontra a unidade e adiciona
                if (incluirUnidadePaiSeChefe && unidadesChefia.Any())
                {
                    var unidadeChefe = items.FirstOrDefault(u => u.UnidadeId == perfisUsuario.Result.UnidadeId);
                    if (unidadeChefe != null)
                    {
                        var unidadePaiChefe = items.FirstOrDefault(u => u.UnidadeId == unidadeChefe.UnidadeIdPai);
                        if (unidadePaiChefe != null)
                            retorno.Add(unidadePaiChefe);
                    }
                }
            }
            return retorno;
        }

        #endregion

        #region Pessoas

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterPessoasComPactoTrabalhoDadosComboAsync(UsuarioLogadoRequest request)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            //Obtém as pessoas
            var dados = await PessoaQuery.ObterComPactoTrabalhoAsync();

            //Filtra os itens de acordo com o perfil do usuário
            var itens = await RestringirPessoasPerfilUsuario(request, dados.Result);

            //Converte de para DadosComboViewModel
            result.Result = itens.Select(u => new DadosComboViewModel() { Id = u.PessoaId.ToString(), Descricao = u.Nome });

            return result;
        }

        private async Task<IEnumerable<PessoaViewModel>> RestringirPessoasPerfilUsuario(UsuarioLogadoRequest request, IEnumerable<PessoaViewModel> items)
        {
            var perfisUsuario = await ObterPerfilPessoaAsync(request);

            //Se não for gestor do sistema, deve filtrar
            if (!perfisUsuario.Result.Perfis.Any(p => p.Perfil == (int)PerfilUsuarioEnum.Gestor))
            {
                //Obtém as unidades em que o usuário é chefe
                var unidadesChefia = (from perfil in perfisUsuario.Result.Perfis
                                      where perfil.Perfil == (int)PerfilUsuarioEnum.ChefeUnidade ||
                                            perfil.Perfil == (int)PerfilUsuarioEnum.CoordenadorGeral ||
                                            perfil.Perfil == (int)PerfilUsuarioEnum.Diretor
                                      from unidade in perfil.Unidades
                                      select unidade).Distinct().ToList();

                //Filtra pelas unidades em que é chefe ou o plano é dele
                items = items.Where(p => unidadesChefia.Any(uu => uu == p.UnidadeId) || p.PessoaId == perfisUsuario.Result.PessoaId);
            }

            return items;
        }

        #endregion

        #region Plano de trabalho

        public async Task<IApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>> ObterPlanoTrabalhoPorFiltroAsync(PlanoTrabalhoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>();

            var unidadesUsuario = await ObterUnidadesPerfilPessoa(request);
            if (unidadesUsuario != null)
                request.UnidadesUsuario = unidadesUsuario.ToList();

            //Obtém as unidades
            var dados = await PlanoTrabalhoQuery.ObterPorFiltroAsync(request);

            result.Result = dados.Result;
            return result;
        }

        public async Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterPlanoTrabalhoPorChaveAsync(PlanoTrabalhoRequest request)
        {
            var result = new ApplicationResult<PlanoTrabalhoViewModel>();

            //Obtém as unidades
            var dados = await PlanoTrabalhoQuery.ObterPorChaveAsync(request.PlanoTrabalhoId);

            //Filtra as unidades de acordo com o perfil do usuário
            var retorno = await RestringirPlanosTrabalhoPerfilUsuario(request, new List<PlanoTrabalhoViewModel>() { dados.Result });
            if (retorno.Any())
                result.Result = dados.Result;

            return result;
        }

        private async Task<IEnumerable<PlanoTrabalhoViewModel>> RestringirPlanosTrabalhoPerfilUsuario(UsuarioLogadoRequest request, IEnumerable<PlanoTrabalhoViewModel> items)
        {
            var perfisUsuario = await ObterPerfilPessoaAsync(request);

            //Se não for gestor do sistema, deve filtrar
            if (!perfisUsuario.Result.Perfis.Any(p => p.Perfil == (int)PerfilUsuarioEnum.Gestor))
            {
                //Obtém as unidades em que o usuário é chefe
                var unidadesChefia = (from perfil in perfisUsuario.Result.Perfis
                                      from unidade in perfil.Unidades
                                      select unidade).Distinct().ToList();

                //Filtra pelas unidades em que é chefe ou o plano é dele
                items = items.Where(p => unidadesChefia.Any(uu => uu == p.UnidadeId) || p.UnidadeId == perfisUsuario.Result.UnidadeId);
            }
            return items;
        }

        #endregion

        #region Pacto de trabalho

        public async Task<IApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>> ObterPactoTrabalhoPorFiltroAsync(PactoTrabalhoFiltroRequest request)
        {
            var result = new ApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>();

            var unidadesUsuario = await ObterUnidadesPerfilPessoa(request);
            if (unidadesUsuario != null)
                request.UnidadesUsuario = unidadesUsuario.ToList();

            //Obtém as unidades
            var dados = await PactoTrabalhoQuery.ObterPorFiltroAsync(request);

            result.Result = dados.Result;
            return result;
        }


        public async Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterPactosTrabalhoPorPlanoAsync(PlanoTrabalhoRequest request)
        {
            var result = new ApplicationResult<IEnumerable<PactoTrabalhoViewModel>>();

            //Obtém as unidades
            var dados = await PactoTrabalhoQuery.ObterPorPlanoAsync(request.PlanoTrabalhoId);

            //Filtra as unidades de acordo com o perfil do usuário
            var retorno = await RestringirPactosTrabalhoPerfilUsuario(request, dados.Result);
            result.Result = retorno;

            return result;
        }

        public async Task<IApplicationResult<PactoTrabalhoViewModel>> ObterPactoTrabalhoPorChaveAsync(PactoTrabalhoRequest request)
        {
            var result = new ApplicationResult<PactoTrabalhoViewModel>();

            //Obtém as unidades
            var dados = await PactoTrabalhoQuery.ObterPorChaveAsync(request.PactoTrabalhoId);

            //Filtra as unidades de acordo com o perfil do usuário
            var retorno = await RestringirPactosTrabalhoPerfilUsuario(request, new List<PactoTrabalhoViewModel>() { dados.Result });
            if (retorno.Any())
                result.Result = dados.Result;

            return result;
        }

        private async Task<IEnumerable<PactoTrabalhoViewModel>> RestringirPactosTrabalhoPerfilUsuario(UsuarioLogadoRequest request, IEnumerable<PactoTrabalhoViewModel> items)
        {
            var perfisUsuario = await ObterPerfilPessoaAsync(request);

            //Se não for gestor do sistema, deve filtrar
            if (!perfisUsuario.Result.Perfis.Any(p => p.Perfil == (int)PerfilUsuarioEnum.Gestor))
            {
                //Se for chefe, obtém as unidades em que o usuário é chefe
                var unidadesChefia = (from perfil in perfisUsuario.Result.Perfis
                                      where perfil.Perfil == (int)PerfilUsuarioEnum.ChefeUnidade
                                      from unidade in perfil.Unidades
                                      select unidade).Distinct().ToList();

                //Filtra pelas unidades em que é chefe ou o plano é dele
                items = items.Where(p => unidadesChefia.Any(uu => uu == p.UnidadeId) || p.PessoaId == perfisUsuario.Result.PessoaId);
            }

            return items;
        }

        #endregion

        #endregion


    }
}
