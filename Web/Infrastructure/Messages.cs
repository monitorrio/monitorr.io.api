using System;

namespace Web.Infrastructure
{
    public interface IEvent { }
    public interface ICommand : IEvent { }
    /// <summary>
    /// An event that is directly related to the entity denoted by Id
    /// </summary>
    public interface IEntityRelatedEvent : IEvent, IUserTriggered
    {
        Guid Id { get; }
    }
    public interface IProvideExplicitDescription
    {
        string ToString();
    }

    public interface IUserTriggeredEvent : IEvent, IUserTriggered { }
    public interface IUserTriggeredCommand : ICommand, IUserTriggered { }

    public interface IUserTriggered
    {
        string User { get; set; }
    }
}