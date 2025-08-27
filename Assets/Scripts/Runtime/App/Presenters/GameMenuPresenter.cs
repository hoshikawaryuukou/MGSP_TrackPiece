using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Presentation.UIWidgets;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameMenuPresenter : IInitializable, IDisposable
    {
        private readonly GameUIStore gameUIStore;
        private readonly GamePlayStore gameStore;
        private readonly GameMenuView menuView;
        private readonly GameInfoView infoView;
        private readonly ConfirmDialogModalView confirmDialogModalView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameMenuPresenter(GameUIStore gameUIStore, GamePlayStore gameStore, GameMenuView menuView, GameInfoView infoView, ConfirmDialogModalView confirmDialogModalView)
        {
            this.gameUIStore = gameUIStore;
            this.gameStore = gameStore;
            this.menuView = menuView;
            this.infoView = infoView;
            this.confirmDialogModalView = confirmDialogModalView;
        }

        void IInitializable.Initialize()
        {
            menuView.RestartRequested.Subscribe(_ => OnRestartRequested()).AddTo(disposables);
            menuView.InfoRequested.Subscribe(_ => OnInfoRequested()).AddTo(disposables);
            menuView.BoardSizeChangeRequested.Subscribe(_ => OnBoardSizeChangeRequested()).AddTo(disposables);
            gameUIStore.LevelRP.Subscribe(OnLevelChanged).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private void OnRestartRequested()
        {
            gameStore.CreateNewGame(gameUIStore.LevelRP.CurrentValue);
        }

        private void OnInfoRequested()
        {
            var dialogDisposables = new CompositeDisposable();
            infoView.CloseRequested
                .Subscribe(_ =>
                {
                    infoView.Hide();
                    dialogDisposables.Dispose();
                    gameUIStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            gameUIStore.SetInteractable(false);
            infoView.Show();
        }

        private void OnBoardSizeChangeRequested()
        {
            var newSize = gameUIStore.LevelRP.CurrentValue == GameLevel._4x4 ? GameLevel._6x6 : GameLevel._4x4;
            var confirmMessage = newSize switch
            {
                GameLevel._4x4 => GameUIConfig.ConfirmRestartWith4x4Message,
                GameLevel._6x6 => GameUIConfig.ConfirmRestartWith6x6Message,
                _ => throw new ArgumentOutOfRangeException(nameof(newSize), newSize, null)
            };

            var dialogDisposables = new CompositeDisposable();
            confirmDialogModalView.YesRequested
                .Subscribe(_ =>
                {
                    confirmDialogModalView.Hide();
                    dialogDisposables.Dispose();
                    gameUIStore.SetInteractable(true);
                    gameStore.CreateNewGame(newSize);
                })
                .AddTo(dialogDisposables);

            confirmDialogModalView.NoRequested
                .Subscribe(_ =>
                {
                    confirmDialogModalView.Hide();
                    dialogDisposables.Dispose();
                    gameUIStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            gameUIStore.SetInteractable(false);
            confirmDialogModalView.SetMessage(confirmMessage);
            confirmDialogModalView.Show();
        }

        private void OnLevelChanged(GameLevel level)
        {
            var message = level switch
            {
                GameLevel._4x4 => "4x4",
                GameLevel._6x6 => "6x6",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
            menuView.SetBoardSizeText(message);
        }
    }
}
