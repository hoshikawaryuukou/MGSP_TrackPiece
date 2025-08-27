using MGSP.TrackPiece.Domain;

namespace MGSP.TrackPiece.App.Events
{
    public sealed class InputRequestedEvent : IGameStageEvent
    {
        public PlayerId CurrentPlayerId { get; }

        public InputRequestedEvent(PlayerId currentPlayerId)
        {
            CurrentPlayerId = currentPlayerId;
        }
    }
}
