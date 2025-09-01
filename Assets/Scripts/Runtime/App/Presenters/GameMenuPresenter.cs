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
        private readonly GameMenuStore gameMenuStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;
        private readonly GameInfoView gameInfoView;
        private readonly ConfirmDialogView confirmDialogModalView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameMenuPresenter(GameMenuStore gameMenuStore, GamePlayStore gamePlayStore, GameMenuView gameMenuView, GameInfoView gameInfoView, ConfirmDialogView confirmDialogModalView)
        {
            this.gameMenuStore = gameMenuStore;
            this.gamePlayStore = gamePlayStore;
            this.gameMenuView = gameMenuView;
            this.gameInfoView = gameInfoView;
            this.confirmDialogModalView = confirmDialogModalView;
        }

        void IInitializable.Initialize()
        {
            gameMenuView.RestartRequested.Subscribe(_ => OnRestartRequested()).AddTo(disposables);
            gameMenuView.InfoRequested.Subscribe(_ => OnInfoRequested()).AddTo(disposables);
            gameMenuView.BoardSizeChangeRequested.Subscribe(_ => OnBoardSizeChangeRequested()).AddTo(disposables);
            gameMenuStore.LevelRP.Subscribe(OnLevelChanged).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private void OnRestartRequested()
        {
            gamePlayStore.CreateNewGame(gameMenuStore.LevelRP.CurrentValue);
        }

        private void OnInfoRequested()
        {
            var dialogDisposables = new CompositeDisposable();
            gameInfoView.CloseRequested
                .Subscribe(_ =>
                {
                    gameInfoView.Hide();
                    dialogDisposables.Dispose();
                    gameMenuStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            gameMenuStore.SetInteractable(false);
            gameInfoView.Show();
        }

        private void OnBoardSizeChangeRequested()
        {
            var newSize = gameMenuStore.LevelRP.CurrentValue == GameLevel._4x4 ? GameLevel._6x6 : GameLevel._4x4;
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
                    gameMenuStore.SetInteractable(true);
                    gamePlayStore.CreateNewGame(newSize);
                })
                .AddTo(dialogDisposables);

            confirmDialogModalView.NoRequested
                .Subscribe(_ =>
                {
                    confirmDialogModalView.Hide();
                    dialogDisposables.Dispose();
                    gameMenuStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            gameMenuStore.SetInteractable(false);
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
            gameMenuView.SetBoardSizeText(message);
        }
    }
}
