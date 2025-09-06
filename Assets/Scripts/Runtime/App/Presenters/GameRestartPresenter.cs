using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameRestartPresenter : IInitializable, IDisposable
    {
        private readonly GameOptionStore gameOptionStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameRestartPresenter(GameOptionStore gameOptionStore, GamePlayStore gamePlayStore, GameMenuView gameMenuView, GameMenuStore gameMenuStore)
        {
            this.gameOptionStore = gameOptionStore;
            this.gamePlayStore = gamePlayStore;
            this.gameMenuView = gameMenuView;
        }

        void IInitializable.Initialize()
        {
            gameMenuView.RestartRequested.Subscribe(_ => OnRestartRequested()).AddTo(disposables);
}

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private void OnRestartRequested()
        {
            gamePlayStore.CreateNewGame(gameOptionStore.LevelRP.CurrentValue);
        }
    }
}
