namespace MGSP.TrackPiece.Services
{
    public sealed class GameStartedEvent : IGameStageEvent
    {
        public GameLevel Level { get; }

        public GameStartedEvent(GameLevel level)
        {
            Level = level;
        }
    }
}
