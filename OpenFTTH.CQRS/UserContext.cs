using System;

namespace OpenFTTH.CQRS
{
    public record UserContext
    {
        public string UserName { get; }
        public Guid WorkTaskId { get; }

        public UserContext(string userName, Guid workTaskId)
        {
            UserName = userName;
            WorkTaskId = workTaskId;
        }
    }
}
