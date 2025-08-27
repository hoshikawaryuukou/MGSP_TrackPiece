using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class PiecesShiftedEvent : IGameStageEvent 
    { 
        public GameLevel Level { get; }

        public PiecesShiftedEvent(GameLevel level)
        {
            Level = level;
        }
    }
}
