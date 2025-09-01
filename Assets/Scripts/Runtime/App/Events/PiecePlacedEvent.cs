using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class PiecePlacedEvent : IGameStageEvent
    {
        public PlayerId PlayerId { get; }
        public int PositionIndex { get; }

        public PiecePlacedEvent(PlayerId playerId, int positionIndex)
        {
            PlayerId = playerId;
            PositionIndex = positionIndex;
        }
    }
}
