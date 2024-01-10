using Lartech.Domain.Core.Messages;
using Lartech.Domain.Core.Messages.CommonMessges;

namespace Lartech.Domain.Core.Comunicacao.Mediator
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarCommand<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}
