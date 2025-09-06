namespace MGSP.TrackPiece.Services
{
    public enum GameResult
    {
        PlayerWhite,
        PlayerBlack,
        Draw
    }

    public sealed class GameEndedEvent : IGameStageEvent
    {
        public GameResult Result { get; }

        public GameEndedEvent(GameResult result)
        {
            Result = result;
        }
    }
}
