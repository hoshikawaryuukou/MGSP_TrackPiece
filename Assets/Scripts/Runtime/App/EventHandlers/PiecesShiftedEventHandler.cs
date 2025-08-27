using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Presentation.StageWidgets;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class PiecesShiftedEventHandler 
    {
        private readonly GameStageView stageView;

        [Inject]
        public PiecesShiftedEventHandler(GameStageView stageView)
        {
            this.stageView = stageView;
        }

        public async UniTask Handle(PiecesShiftedEvent evt)
        {
            await stageView.Shift(GameConfigTable.GetConfig(evt.Level).Track);
        }
    }
}
