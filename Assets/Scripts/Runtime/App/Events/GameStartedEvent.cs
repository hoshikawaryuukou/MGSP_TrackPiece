using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class GameStartedEvent : IGameStageEvent
    {
        public GameLevel Level { get; }
        public PlayerId StartingPlayerId { get; }

        public GameStartedEvent(GameLevel level, PlayerId startingPlayerId)
        {
            Level = level;
            StartingPlayerId = startingPlayerId;
        }
    }
}
