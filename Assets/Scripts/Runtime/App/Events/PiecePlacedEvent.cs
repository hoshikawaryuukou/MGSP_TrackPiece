using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class PiecePlacedEvent : IGameStageEvent
    {
        public PlayerId PlayerId { get; }
        public int Position { get; }

        public PiecePlacedEvent(PlayerId playerId, int position)
        {
            PlayerId = playerId;
            Position = position;
        }
    }
}
