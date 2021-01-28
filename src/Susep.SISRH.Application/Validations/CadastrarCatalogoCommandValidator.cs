using FluentValidation;
using Susep.SISRH.Application.Commands.Catalogo;

namespace Susep.SISRH.Application.Validations
{
    public class CadastrarCatalogoCommandValidator : AbstractValidator<CadastrarCatalogoCommand>
    {

        public CadastrarCatalogoCommandValidator()
        {

            RuleFor(c => c.UnidadeId)
                .NotNull().NotEmpty().WithMessage("A unidade deve ser informada");

        }

    }
}
