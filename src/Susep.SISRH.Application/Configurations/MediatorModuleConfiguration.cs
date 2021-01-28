using Autofac;
using FluentValidation;
using MediatR;
using Susep.SISRH.Application.Commands;
using Susep.SISRH.Application.Commands.Catalogo;
using Susep.SISRH.Application.Validations;
using SUSEP.Framework.CoreFilters.Behaviors;
using System.Reflection;

namespace Susep.SISRH.Application.Configurations
{
    public class MediatorModuleConfiguration : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(CadastrarCatalogoCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(CadastrarCatalogoCommandValidator).GetTypeInfo().Assembly).Where(it => it.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(SituacaoCorretorAlteradaDomainEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                IComponentContext componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}

