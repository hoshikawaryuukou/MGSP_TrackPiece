using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Presentation.StageWidgets;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class PiecePlacedEventHandler
    {
        private readonly GameStageView gameStageView;

        [Inject]
        public PiecePlacedEventHandler(GameStageView gameStageView)
        {
            this.gameStageView = gameStageView;
        }

        public void Handle(PiecePlacedEvent evt)
        {
            var pieceType = evt.PlayerId == Domain.PlayerId.Player1 ? PieceType.WHITE : PieceType.BLACK;

            gameStageView.Place(evt.PositionIndex, pieceType);
        }
    }
}
