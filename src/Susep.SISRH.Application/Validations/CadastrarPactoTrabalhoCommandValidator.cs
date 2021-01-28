using FluentValidation;
using Susep.SISRH.Application.Commands.PactoTrabalho;
using Susep.SISRH.Application.Commands.PlanoTrabalho;
using Susep.SISRH.Domain.Enums;
using System;

namespace Susep.SISRH.Application.Validations
{
    public class CadastrarPactoTrabalhoCommandValidator : AbstractValidator<CadastrarPactoTrabalhoCommand>
    {

        public CadastrarPactoTrabalhoCommandValidator()
        {

            RuleFor(c => c.DataInicio)
                .GreaterThanOrEqualTo(c => DateTime.Now.Date).WithMessage("A data de início deve ser maior que a data atual");

            RuleFor(c => c.DataFim)
                .GreaterThanOrEqualTo(c => c.DataInicio).WithMessage("A data de fim deve ser maior que a data de início");

        }

    }
}
