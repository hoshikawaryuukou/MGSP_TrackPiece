using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Presentation.StageWidgets;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class PiecePlacedEventHandler
    {
        private readonly GameStageView stageView;

        [Inject]
        public PiecePlacedEventHandler(GameStageView stageView)
        {
            this.stageView = stageView;
        }

        public void Handle(PiecePlacedEvent evt)
        {
            var pieceType = evt.PlayerId == Domain.PlayerId.Player1 ? PieceType.WHITE : PieceType.BLACK;

            stageView.Place(evt.Position, pieceType);
        }
    }
}
