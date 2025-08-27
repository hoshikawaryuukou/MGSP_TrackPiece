using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Presentation.StageWidgets;
using MGSP.TrackPiece.Presentation.UIWidgets;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class GameStartedEventHandler
    {
        private readonly GameUIStore gameUIStore;
        private readonly GameResultBannerView resultBannerView;
        private readonly GameStageView stageView;

        [Inject]
        public GameStartedEventHandler(GameResultBannerView resultBannerView, GameStageView stageView, GameUIStore gameUIStore)
        {
            this.resultBannerView = resultBannerView;
            this.stageView = stageView;
            this.gameUIStore = gameUIStore;
        }

        public void Handle(GameStartedEvent evt)
        {
            gameUIStore.SetLevel(evt.Level);

            resultBannerView.Hide();
            stageView.Arrange(GameConfigTable.GetConfig(evt.Level).LevelSize);
        }
    }
}
