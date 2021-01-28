using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SUSEP.Framework.MessageBroker.Abstractions;
using SUSEP.Framework.SeedWorks.Events;

namespace Susep.SISRH.Application.Configurations
{
    public static class EventBusConfiguration
    {
        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            //IOptions<CriarUsuarioOptions> criarUsuarioOptions = app.ApplicationServices.GetRequiredService<IOptions<CriarUsuarioOptions>>();
            ////eventBus.SubscribeAsync<CriarUsuarioIntegrationEvent, IIntegrationEventHandler<CriarUsuarioIntegrationEvent>>(operacaoHandler.Value);
            //eventBus.SubscribeServerAsync<CriarUsuarioRequestEvent, DadosUsuarioResponseEvent, IIntegrationEventHandler<CriarUsuarioRequestEvent, DadosUsuarioResponseEvent>>(criarUsuarioOptions.Value);

        }
    }
}
