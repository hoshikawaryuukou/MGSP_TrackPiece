using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.UIWidgets;
using MGSP.TrackPiece.Services;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
{
    public sealed class GameMenuLevelChangePresenter : IInitializable, IDisposable, IInputBlocker
    {
        private readonly GameUIStateStore uiStateStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameMenuView gameMenuView;
        private readonly ConfirmDialogView confirmDialogView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameMenuLevelChangePresenter(GameUIStateStore uiStateStore, GamePlayStore gamePlayStore, GameMenuView gameMenuView, ConfirmDialogView confirmDialogView)
        {
            this.uiStateStore = uiStateStore;
            this.gamePlayStore = gamePlayStore;
            this.gameMenuView = gameMenuView;
            this.confirmDialogView = confirmDialogView;
        }

        void IInitializable.Initialize()
        {
            gamePlayStore.LevelRP.Subscribe(OnLevelChanged).AddTo(disposables);

            gameMenuView.BoardSizeChangeRequested.Subscribe(_ => Show()).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private void Show()
        {
            uiStateStore.RequestInputBlock(this);

            var newSize = gamePlayStore.LevelRP.CurrentValue == GameLevel._4x4 ? GameLevel._6x6 : GameLevel._4x4;
            var confirmMessage = newSize switch
            {
                GameLevel._4x4 => GameUIConfig.ConfirmRestartWith4x4Message,
                GameLevel._6x6 => GameUIConfig.ConfirmRestartWith6x6Message,
                _ => throw new ArgumentOutOfRangeException(nameof(newSize), newSize, null)
            };

            var dialogDisposables = new CompositeDisposable();
            confirmDialogView.YesRequested
                .Subscribe(_ =>
                {
                    confirmDialogView.Hide();
                    dialogDisposables.Dispose();

                    gamePlayStore.SetLevel(newSize);
                    gamePlayStore.CreateNewGame(newSize).Forget();
                    uiStateStore.ReleaseInputBlock(this);
                })
                .AddTo(dialogDisposables);

            confirmDialogView.NoRequested
                .Subscribe(_ =>
                {
                    confirmDialogView.Hide();
                    dialogDisposables.Dispose();

                    uiStateStore.ReleaseInputBlock(this);
                })
                .AddTo(dialogDisposables);

            confirmDialogView.SetMessage(confirmMessage);
            confirmDialogView.Show();
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
