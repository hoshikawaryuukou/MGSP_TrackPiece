namespace MGSP.TrackPiece.App.Events
{
    public sealed class InputInvalidEvent : IGameStageEvent
    {
        public string Reason { get; }
        public int AttemptedPosition { get; }

        public InputInvalidEvent(string reason, int attemptedPosition)
        {
            Reason = reason;
            AttemptedPosition = attemptedPosition;
        }
    }
}
