using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.Presentation.StageWidgets;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Shared;
using MGSP.TrackPiece.Stores;
using R3;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GamePlayPresenter : IPostStartable, IDisposable
    {
        private readonly GamePlayStore gamePlayStore;
        private readonly GameOptionStore gameMenuStore;
        private readonly GameStageView gameStageView;
        private readonly GameResultView gameResultView;
        private readonly CellViewSelector cellViewSelector;

        private readonly CancellationTokenSource cts = new();

        [Inject]
        public GamePlayPresenter(GamePlayStore gamePlayStore, GameOptionStore gameMenuStore, GameStageView gameStageView, GameResultView gameResultView, CellViewSelector cellViewSelector)
        {
            this.gamePlayStore = gamePlayStore;
            this.gameMenuStore = gameMenuStore;
            this.gameStageView = gameStageView;
            this.gameResultView = gameResultView;
            this.cellViewSelector = cellViewSelector;
        }

        void IPostStartable.PostStart()
        {
            Run(cts.Token).Forget();
        }

        void IDisposable.Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }

        private async UniTask Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var evt = gamePlayStore.ReadEvent();
                if (evt != null)
                {
                    switch (evt)
                    {
                        case GameStartedEvent e: OnGameStarted(e); break;
                        case GameEndedEvent e: OnGameEnded(e); break;
                        case TurnStartedEvent e: await OnTurnStarted(e); break;
                        case TurnEndedEvent e: await OnTurnEnded(e); break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                await UniTask.Yield();
            }
        }

        private void OnGameStarted(GameStartedEvent evt)
        {
            gameResultView.Hide();

            var config = GetConfigByGameLevel(gameMenuStore.LevelRP.CurrentValue);

            gameStageView.Arrange(config.BoardSizeLength);
        }

        private void OnGameEnded(GameEndedEvent evt)
        {
            var resultMessage = evt.Result switch
            {
                GameResult.PlayerWhite => GameUIConfig.PlayerWhiteWinMessage,
                GameResult.PlayerBlack => GameUIConfig.PlayerBlackWinMessage,
                GameResult.Draw => GameUIConfig.DrawMessage,
                _ => throw new InvalidOperationException("Unexpected game result."),
            };

            gameResultView.Show(resultMessage);
        }

        private async UniTask OnTurnStarted(TurnStartedEvent evt)
        {
            try
            {
                var positionIndex = await cellViewSelector.CellViewSelected
                    .Where(cellView => gamePlayStore.IsInteractableRP.CurrentValue)
                    .Select(cellView => cellView.positionIndex)
                    .Where(positionIndex => evt.AvailablePositions[positionIndex])
                    .FirstAsync(gamePlayStore.CancellationToken)
                    .AsUniTask();

                gamePlayStore.Place(positionIndex);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Cancel");
            }
        }

        private async UniTask OnTurnEnded(TurnEndedEvent evt)
        {
            var pieceType = evt.Player == GamePlayer.White ? PieceType.WHITE : PieceType.BLACK;

            gameStageView.Place(evt.PositionIndex, pieceType);

            var config = GetConfigByGameLevel(gameMenuStore.LevelRP.CurrentValue);

            await gameStageView.Shift(config.Track);
        }

        private GameLevelConfig GetConfigByGameLevel(GameLevel gameLevel)
        {
            return gameLevel == GameLevel._4x4
                ? GameLevelConfigTable.Config4x4
                : GameLevelConfigTable.Config6x6;
        }
    }
}
