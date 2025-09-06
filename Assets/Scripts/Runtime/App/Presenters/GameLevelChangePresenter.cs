using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameLevelChangePresenter : IInitializable, IDisposable
    {
        private readonly GameMenuStore gameMenuStore;
        private readonly GameOptionStore gameOptionStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;
        private readonly ConfirmDialogView confirmDialogModalView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameLevelChangePresenter(GameMenuStore gameMenuStore, GameOptionStore gameOptionStore, GamePlayStore gamePlayStore, GameMenuView gameMenuView, ConfirmDialogView confirmDialogModalView)
        {
            this.gameMenuStore = gameMenuStore;
            this.gameOptionStore = gameOptionStore;
            this.gamePlayStore = gamePlayStore;
            this.gameMenuView = gameMenuView;
            this.confirmDialogModalView = confirmDialogModalView;
        }

        void IInitializable.Initialize()
        {
            gameMenuStore.LevelChangeRequested.Subscribe(_ => Show()).AddTo(disposables);
            gameOptionStore.LevelRP.Subscribe(OnLevelChanged).AddTo(disposables);

            gameMenuView.BoardSizeChangeRequested.Subscribe(_ => gameMenuStore.RequestLevelChange()).AddTo(disposables);

        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private void Show()
        {
            var newSize = gameOptionStore.LevelRP.CurrentValue == GameLevel._4x4 ? GameLevel._6x6 : GameLevel._4x4;
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

                    gameOptionStore.SetLevel(newSize);
                    gamePlayStore.CreateNewGame(newSize);
                    gamePlayStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            confirmDialogModalView.NoRequested
                .Subscribe(_ =>
                {
                    confirmDialogModalView.Hide();
                    dialogDisposables.Dispose();
                    gamePlayStore.SetInteractable(true);
                })
                .AddTo(dialogDisposables);

            gamePlayStore.SetInteractable(false);
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
