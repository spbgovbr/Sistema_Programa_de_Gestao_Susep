using FluentValidation;
using Susep.SISRH.Application.Commands.PactoTrabalho;
using Susep.SISRH.Application.Commands.PlanoTrabalho;
using Susep.SISRH.Domain.Enums;
using System;

namespace Susep.SISRH.Application.Validations
{
    public class AlterarAndamentoPactoTrabalhoAtividadeCommandValidator : AbstractValidator<AlterarAndamentoPactoTrabalhoAtividadeCommand>
    {

        public AlterarAndamentoPactoTrabalhoAtividadeCommandValidator()
        {

            RuleFor(c => c.DataInicio)
                .LessThanOrEqualTo(c => DateTime.Now).WithMessage("A data de início deve ser menor que a data atual");

            RuleFor(c => c.DataFim)
                .LessThan(c => DateTime.Now).WithMessage("A data de fim deve ser menor que a data atual")
                .GreaterThanOrEqualTo(c => c.DataInicio).WithMessage("A data de fim deve ser maior que a data de início");

        }

    }
}
