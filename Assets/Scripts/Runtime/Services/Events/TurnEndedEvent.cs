namespace MGSP.TrackPiece.Services.Events
{
    public sealed class TurnEndedEvent : IGameStageEvent
    {
        public GamePlayer Player { get; }
        public int PositionIndex { get; }

        public TurnEndedEvent(GamePlayer player, int positionIndex)
        {
            Player = player;
            PositionIndex = positionIndex;
        }
    }
}
