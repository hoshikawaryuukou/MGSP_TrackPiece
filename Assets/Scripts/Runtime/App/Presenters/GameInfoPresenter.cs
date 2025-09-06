using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameInfoPresenter : IInitializable, IDisposable
    {
        private readonly GameMenuStore gameMenuStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;
        private readonly GameInfoView gameInfoView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameInfoPresenter(GameMenuStore gameMenuStore, GamePlayStore gamePlayStore, GameInfoView gameInfoView, GameMenuView gameMenuView)
        {
            this.gameMenuStore = gameMenuStore;
            this.gamePlayStore = gamePlayStore;
            this.gameInfoView = gameInfoView;
            this.gameMenuView = gameMenuView;
        }

        void IInitializable.Initialize()
        {
            gameMenuStore.InfoOnRP
                .Subscribe(isOn =>
                {
                    if (isOn)
                    {
                        gameInfoView.Show();
                        gamePlayStore.SetInteractable(false);
                    }
                    else
                    {
                        gameInfoView.Hide();
                        gamePlayStore.SetInteractable(true);
                    }
                })
                .AddTo(disposables);

            gameMenuView.InfoRequested.Subscribe(_ => gameMenuStore.SetInfoOn(true)).AddTo(disposables);
            gameInfoView.CloseRequested.Subscribe(_ => { gameMenuStore.SetInfoOn(false); }).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}
