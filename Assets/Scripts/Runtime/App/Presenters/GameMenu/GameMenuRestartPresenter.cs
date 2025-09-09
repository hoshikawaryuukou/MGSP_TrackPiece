using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameMenuRestartPresenter : IInitializable, IDisposable
    {
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameMenuRestartPresenter( GamePlayStore gamePlayStore, GameMenuView gameMenuView)
        {
            this.gamePlayStore = gamePlayStore;
            this.gameMenuView = gameMenuView;
        }

        void IInitializable.Initialize()
        {
            gameMenuView.RestartRequested
                .Subscribe(_ => gamePlayStore.CreateNewGame(gamePlayStore.LevelRP.CurrentValue).Forget())
                .AddTo(disposables);
}

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}
