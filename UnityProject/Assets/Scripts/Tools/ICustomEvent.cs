namespace Clovers.Tools
{
    /// <summary>
    /// Event interface to integrate the identifier of an event to the event object.
    /// </summary>
    public interface ICustomEvent
    {
        EventIdentifier Identifier { get; }
    }
}