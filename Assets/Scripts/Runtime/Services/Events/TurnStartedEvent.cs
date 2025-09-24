using System.Collections.Generic;

namespace MGSP.TrackPiece.Services.Events
{
    public sealed class TurnStartedEvent : IGameStageEvent
    {
        public GamePlayer Player { get; }
        public IReadOnlyList<bool> AvailablePositions { get; }

        public TurnStartedEvent(GamePlayer player, IReadOnlyList<bool> availablePositions)
        {
            Player = player;
            AvailablePositions = availablePositions;
        }
    }
}
