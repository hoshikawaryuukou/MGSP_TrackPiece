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
        private readonly GameMenuStore gameMenuStore;
        private readonly GameResultView gameResultView;
        private readonly GameStageView gameStageView;

        [Inject]
        public GameStartedEventHandler(GameMenuStore gameMenuStore, GameResultView gameResultView, GameStageView gameStageView)
        {
            this.gameMenuStore = gameMenuStore;
            this.gameResultView = gameResultView;
            this.gameStageView = gameStageView;
        }

        public void Handle(GameStartedEvent evt)
        {
            gameResultView.Hide();

            gameMenuStore.SetLevel(evt.Level);
            gameStageView.Arrange(GameConfigTable.GetConfig(evt.Level).LevelSize);
        }
    }
}
