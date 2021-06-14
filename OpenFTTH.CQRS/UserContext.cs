using System;

namespace OpenFTTH.CQRS
{
    public record UserContext
    {
        public string UserName { get; }
        public Guid WorkTaskId { get; }
        public Guid EditingRouteNodeId { get; init; }

        public UserContext(string userName, Guid workTaskId)
        {
            UserName = userName;
            WorkTaskId = workTaskId;
        }
    }
}
