using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.UIWidgets;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
{
    public sealed class GameMenuInfoPresenter : IInitializable, IDisposable, IInputBlocker
    {
        private readonly GameUIStateStore uiStateStore;
        private readonly GameMenuView gameMenuView;
        private readonly GameInfoView gameInfoView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameMenuInfoPresenter(GameUIStateStore uiStateStore, GameMenuView gameMenuView, GameInfoView gameInfoView)
        {
            this.uiStateStore = uiStateStore;
            this.gameMenuView = gameMenuView;
            this.gameInfoView = gameInfoView;
        }

        void IInitializable.Initialize()
        {
            gameMenuView.InfoRequested
                .Subscribe(_ => 
                {
                    gameInfoView.Show();
                    uiStateStore.RequestInputBlock(this);

                }).AddTo(disposables);

            gameInfoView.CloseRequested
                .Subscribe(_ => 
                {
                    gameInfoView.Hide();
                    uiStateStore.ReleaseInputBlock(this);

                }).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}
