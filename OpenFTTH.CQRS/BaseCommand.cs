using System;

namespace OpenFTTH.CQRS
{
    public abstract record BaseCommand
    {
        public Guid? CmdId { get; init; }
        public DateTime Timestamp { get; init; }
        public Guid? IncitingEventId { get; init; }
        public string? UserName { get; init; }
        public Guid? WorkTaskId { get; init; }
    }
}
