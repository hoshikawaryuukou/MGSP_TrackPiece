using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Presentation.StageWidgets;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class PiecesShiftedEventHandler 
    {
        private readonly GameStageView gameStageView;

        [Inject]
        public PiecesShiftedEventHandler(GameStageView gameStageView)
        {
            this.gameStageView = gameStageView;
        }

        public async UniTask Handle(PiecesShiftedEvent evt)
        {
            await gameStageView.Shift(GameConfigTable.GetConfig(evt.Level).Track);
        }
    }
}
