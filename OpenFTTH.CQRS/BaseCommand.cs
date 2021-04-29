using System;

namespace OpenFTTH.CQRS
{
    public abstract record BaseCommand
    {
        public Guid CmdId { get; init; }
        public DateTime Timestamp { get; init; }
        public UserContext UserContext { get; init; }
    }
}
