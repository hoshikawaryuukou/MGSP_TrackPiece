namespace MGSP.TrackPiece.App.Events
{
    public sealed class InputInvalidEvent : IGameStageEvent
    {
        public string Reason { get; }
        public int AttemptedPositionIndex { get; }

        public InputInvalidEvent(string reason, int attemptedPositionIndex)
        {
            Reason = reason;
            AttemptedPositionIndex = attemptedPositionIndex;
        }
    }
}
