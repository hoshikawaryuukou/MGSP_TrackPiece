namespace MGSP.TrackPiece.Services
{
    public enum GameResult
    {
        PlayerWhite,
        PlayerBlack,
        Draw
    }
}

namespace MGSP.TrackPiece.Services.Events
{
    public sealed class RoundEndedEvent : IGameStageEvent
    {
        public GameResult Result { get; }

        public RoundEndedEvent(GameResult result)
        {
            Result = result;
        }
    }
}