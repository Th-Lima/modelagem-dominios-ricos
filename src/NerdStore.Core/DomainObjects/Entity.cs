using System.Reflection.Metadata;
using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }

    private List<Event> _eventNotificacoes;
    public IReadOnlyCollection<Event> EventNotificacoes => _eventNotificacoes?.AsReadOnly();
    
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public void AdicionarEvento(Event evento)
    {
        _eventNotificacoes = _eventNotificacoes ?? new List<Event>();
        _eventNotificacoes.Add(evento);
    }

    public void RemoverEvento(Event eventItem)
    {
        _eventNotificacoes?.Remove(eventItem);
    }

    public void LimparEventos()
    {
        _eventNotificacoes?.Clear();
    }
    
    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;
        
        if (ReferenceEquals(null, compareTo))
            return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }
    
    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907 + Id.GetHashCode());
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }
}