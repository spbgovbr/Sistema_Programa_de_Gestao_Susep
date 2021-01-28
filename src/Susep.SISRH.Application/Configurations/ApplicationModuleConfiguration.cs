using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using Susep.SISRH.Infrastructure.Repositories;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Data.Concrete.UnitOfWorks;
using SUSEP.Framework.Utils.Abstractions;
using SUSEP.Framework.Utils.Helpers;

namespace Susep.SISRH.Application.Configurations
{
    public class ApplicationModuleConfiguration : Autofac.Module
    {
        private IConfiguration Configuration { get; }

        public ApplicationModuleConfiguration() { }
        public ApplicationModuleConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Registra o DbContextOptionsBuilder
            builder.Register(ctx =>
            {
                DbContextOptionsBuilder<SISRHDbContext> options = new DbContextOptionsBuilder<SISRHDbContext>();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

                return new SISRHDbContext(options.Options);
            })
            .As<SISRHDbContext>()
            .InstancePerLifetimeScope();

            //Registra as queries            
            builder.RegisterType<AtividadeQuery>().As<IAtividadeQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CatalogoQuery>().As<ICatalogoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<DominioQuery>().As<IDominioQuery>().InstancePerLifetimeScope();
            builder.RegisterType<ItemCatalogoQuery>().As<IItemCatalogoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<PactoTrabalhoQuery>().As<IPactoTrabalhoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<PessoaQuery>().As<IPessoaQuery>().InstancePerLifetimeScope();
            builder.RegisterType<PlanoTrabalhoQuery>().As<IPlanoTrabalhoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<UnidadeQuery>().As<IUnidadeQuery>().InstancePerLifetimeScope();
            builder.RegisterType<EstruturaOrganizacionalQuery>().As<IEstruturaOrganizacionalQuery>().InstancePerLifetimeScope();
            builder.RegisterType<AssuntoQuery>().As<IAssuntoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<ObjetoQuery>().As<IObjetoQuery>().InstancePerLifetimeScope();
            builder.RegisterType<APIExtracaoOrgaoCentralQuery>().As<IAPIExtracaoOrgaoCentralQuery>().InstancePerLifetimeScope();

            //Registra os commands
            builder.RegisterType<CatalogoRepository>().As<ICatalogoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ItemCatalogoRepository>().As<IItemCatalogoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PessoaRepository>().As<IPessoaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PactoTrabalhoRepository>().As<IPactoTrabalhoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PlanoTrabalhoRepository>().As<IPlanoTrabalhoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AssuntoRepository>().As<IAssuntoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ObjetoRepository>().As<IObjetoRepository>().InstancePerLifetimeScope();

            builder.RegisterType<EmailHelper>().As<IEmailHelper>().InstancePerLifetimeScope();

            //Registra os integration events
            //builder.RegisterAssemblyTypes(typeof(EnvioResultadoIntegrationEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IIntegrationEventHandler<,>));

            //Registra o DbContext
            builder.Register(ctx =>
            {
                DbContext context = ctx.Resolve<SISRHDbContext>();
                IMediator mediator = ctx.Resolve<IMediator>();

                return new SqlServerUOW(context, mediator);
            })
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                return Configuration;
            })
            .As<IConfiguration>()
            .InstancePerLifetimeScope();

            //builder.Register(ctx =>
            //{
            //    IOptions<BrokerOptions> broker = ctx.Resolve<IOptions<BrokerOptions>>();
            //    IComponentContext context = ctx.Resolve<IComponentContext>();

            //    return new RabbitMQEventBus(context, broker);
            //})
            //.As<IEventBus>()
            //.InstancePerLifetimeScope();
        }
    }
}
