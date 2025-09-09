using Cysharp.Threading.Tasks;
using MessagePipe;
using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.Presentation.StageWidgets;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using System.Threading;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GamePlayRoundPresenter : IInitializable, IDisposable
    {
        private readonly IAsyncSubscriber<RoundStartedEvent> gameStartedSubscriber;
        private readonly IAsyncSubscriber<RoundEndedEvent> gameEndedSubscriber;
        private readonly GamePlayStore gamePlayStore;
        private readonly GameStageView gameStageView;
        private readonly GameResultView gameResultView;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GamePlayRoundPresenter(IAsyncSubscriber<RoundStartedEvent> gameStartedSubscriber, IAsyncSubscriber<RoundEndedEvent> gameEndedSubscriber, GamePlayStore gamePlayStore, GameStageView gameStageView, GameResultView gameResultView)
        {
            this.gameStartedSubscriber = gameStartedSubscriber;
            this.gameEndedSubscriber = gameEndedSubscriber;
            this.gamePlayStore = gamePlayStore;
            this.gameStageView = gameStageView;
            this.gameResultView = gameResultView;
        }

        void IInitializable.Initialize()
        {
            gameStartedSubscriber.Subscribe(OnGameStarted).AddTo(disposables);
            gameEndedSubscriber.Subscribe(OnGameEnded).AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private UniTask OnGameStarted(RoundStartedEvent evt, CancellationToken cancellationToken)
        {
            gameResultView.Hide();
            gameStageView.Arrange(gamePlayStore.LevelConfig.BoardSideLength);
            return UniTask.CompletedTask;
        }

        private UniTask OnGameEnded(RoundEndedEvent evt, CancellationToken cancellationToken)
        {
            var resultMessage = evt.Result switch
            {
                GameResult.PlayerWhite => GameUIConfig.PlayerWhiteWinMessage,
                GameResult.PlayerBlack => GameUIConfig.PlayerBlackWinMessage,
                GameResult.Draw => GameUIConfig.DrawMessage,
                _ => throw new InvalidOperationException("Unexpected game result."),
            };

            gameResultView.Show(resultMessage);
            return UniTask.CompletedTask;
        }

    }
}
