using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class GameEndedEvent : IGameStageEvent
    {
        public GameResult Result { get; }

        public GameEndedEvent(GameResult result)
        {
            Result = result;
        }
    }
}
