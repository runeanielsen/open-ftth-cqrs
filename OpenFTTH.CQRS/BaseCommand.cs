using System;

namespace OpenFTTH.CQRS
{
    public abstract record BaseCommand
    {
        public Guid CmdId { get;  }
        public Guid CorrelationId { get; }
        public DateTime Timestamp { get;  }
        public UserContext UserContext { get; }

        protected BaseCommand(Guid correlationId, UserContext userContext)
        {
            CmdId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;

            CorrelationId = correlationId;
            UserContext = userContext;
        }
    }
}
