using Cysharp.Threading.Tasks;
using MessagePipe;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.StageWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Services.Events;
using R3;
using System;
using System.Threading;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
{
    public sealed class GamePlayTrunPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<TurnStartedEvent> turnStartedSubscriber;
        private readonly IAsyncSubscriber<TurnEndedEvent> turnEndedSubscriber;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameStageView gameStageView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GamePlayTrunPresenter(IAsyncSubscriber<TurnStartedEvent> turnStartedSubscriber, IAsyncSubscriber<TurnEndedEvent> turnEndedSubscriber, GamePlayStore gamePlayStore, GameStageView gameStageView)
        {
            this.turnStartedSubscriber = turnStartedSubscriber;
            this.turnEndedSubscriber = turnEndedSubscriber;
            this.gamePlayStore = gamePlayStore;
            this.gameStageView = gameStageView;
        }

        void IInitializable.Initialize()
        {
            turnStartedSubscriber.Subscribe(OnTurnStarted).AddTo(disposables);
            turnEndedSubscriber.Subscribe(OnTurnEnded).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private UniTask OnTurnStarted(TurnStartedEvent evt, CancellationToken cancellationToken)
        {
            gameStageView.SetCellViewsInteractable(evt.AvailablePositions);

            return UniTask.CompletedTask;
        }

        private async UniTask OnTurnEnded(TurnEndedEvent evt, CancellationToken cancellationToken)
        {
            var pieceType = evt.Player == GamePlayer.White ? PieceType.WHITE : PieceType.BLACK;

            gameStageView.Place(evt.PositionIndex, pieceType);

            await gameStageView.Shift(gamePlayStore.LevelConfig.Track);
        }
    }
}
