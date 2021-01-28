using FluentValidation;
using Susep.SISRH.Application.Commands.PlanoTrabalho;
using Susep.SISRH.Domain.Enums;

namespace Susep.SISRH.Application.Validations
{
    public class AlterarPlanoTrabalhoCommandValidator : AbstractValidator<AlterarPlanoTrabalhoCommand>
    {

        public AlterarPlanoTrabalhoCommandValidator()
        {

            //RuleFor(c => c.Titulo)
            //    .NotNull().NotEmpty().WithMessage("O título deve ser informado")
            //    .MaximumLength(250).WithMessage("O título deve ter no máximo 250 caracteres");

            //RuleFor(c => c.Descricao).MaximumLength(2000).WithMessage("A descrição deve ter no máximo 2000 caracteres");

            //RuleFor(c => c.FormaCalculoTempoPlanoTrabalhoId)
            //    .NotNull().NotEmpty().WithMessage("A forma de cálculo do tempo deve ser informada");

            //RuleFor(c => c.PermiteTrabalhoRemoto)
            //    .NotNull().WithMessage("Deve ser informado se permite trabalho remoto");

            //RuleFor(c => c.TempoExecucaoPresencial)
            //    .NotNull().GreaterThan(0)
            //        .When(c => c.FormaCalculoTempoPlanoTrabalhoId != (int)FormaCalculoTempoPlanoTrabalhoEnum.PosdefinidoPorTarefa)
            //        .WithMessage("O tempo de execução presencial é obrigatório");

            //RuleFor(c => c.TempoExecucaoRemoto)
            //    .NotNull().GreaterThan(0)
            //        .When(c => c.FormaCalculoTempoPlanoTrabalhoId != (int)FormaCalculoTempoPlanoTrabalhoEnum.PosdefinidoPorTarefa &&
            //                   c.PermiteTrabalhoRemoto)
            //        .WithMessage("O tempo de execução presencial é obrigatório");
        }

    }
}
