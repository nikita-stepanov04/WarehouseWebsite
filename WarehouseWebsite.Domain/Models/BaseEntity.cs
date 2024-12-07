using MediatR;
using System.Text.Json.Serialization;

namespace WarehouseWebsite.Domain.Models
{
    public class BaseEntity
    {
        public Guid Id { get; init; }

        [JsonIgnore]
        private readonly List<INotification> _domainEvents = new();

        [JsonIgnore]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void RaiseDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
