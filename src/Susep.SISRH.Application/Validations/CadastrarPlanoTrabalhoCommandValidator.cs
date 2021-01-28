using FluentValidation;
using Susep.SISRH.Application.Commands.PlanoTrabalho;
using Susep.SISRH.Domain.Enums;

namespace Susep.SISRH.Application.Validations
{
    public class CadastrarPlanoTrabalhoCommandValidator : AbstractValidator<CadastrarPlanoTrabalhoCommand>
    {

        public CadastrarPlanoTrabalhoCommandValidator()
        {

            RuleFor(c => c.UnidadeId)
                .NotNull().NotEmpty().WithMessage("A unidade deve ser informada");

            RuleFor(c => c.DataInicio)
                .NotNull().NotEmpty().WithMessage("A data de início deve ser informada");

            RuleFor(c => c.DataFim)
                .NotNull().NotEmpty().WithMessage("A data de fim deve ser informada");

        }

    }
}
