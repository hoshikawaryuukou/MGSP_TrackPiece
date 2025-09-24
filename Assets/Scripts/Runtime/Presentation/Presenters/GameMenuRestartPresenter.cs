using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.UIWidgets;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
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
