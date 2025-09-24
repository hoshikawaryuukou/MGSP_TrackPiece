namespace MGSP.TrackPiece.Services.Events
{
    public sealed class RoundStartedEvent : IGameStageEvent
    {
        public GameLevel Level { get; }

        public RoundStartedEvent(GameLevel level)
        {
            Level = level;
        }
    }
}
