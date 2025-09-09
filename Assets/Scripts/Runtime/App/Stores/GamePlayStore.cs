using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Services;
using R3;
using System;
using System.Collections.Generic;
using System.Threading;
using VContainer;

namespace MGSP.TrackPiece.Stores
{
    public enum GameStatus
    {
        None,
        ResponseWait,
        StageDisplay,
        PlayerAction,
    }

    public sealed class GamePlayStore : IDisposable
    {
        private readonly IGameService gameService;
        private readonly GameStageEventEmitter eventEmitter;

        private readonly ReactiveProperty<GameLevel> levelRP = new(GameLevel._4x4);
        private readonly ReactiveProperty<GameStatus> statusRP = new(GameStatus.None);
        private CancellationTokenSource cts = new();

        public ReadOnlyReactiveProperty<GameLevel> LevelRP => levelRP;
        public ReadOnlyReactiveProperty<GameStatus> StatusRP => statusRP;
        public GameLevelConfig LevelConfig => GameLevelConfigTable.Table[levelRP.Value];

        [Inject]
        public GamePlayStore(IGameService gameService, GameStageEventEmitter eventEmitter)
        {
            this.gameService = gameService;
            this.eventEmitter = eventEmitter;
        }

        void IDisposable.Dispose()
        {
            cts.Dispose();
        }

        public void SetLevel(GameLevel level)
        {
            levelRP.Value = level;
        }

        public async UniTask CreateNewGame(GameLevel level)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();

            statusRP.Value = GameStatus.ResponseWait;

            var response = await gameService.CreateNewGame(level);
            await ProcessEvents(response, cts.Token);
        }

        public async UniTask Place(int positionIndex)
        {
            statusRP.Value = GameStatus.ResponseWait;

            var response = await gameService.PlacePiece(positionIndex);
            await ProcessEvents(response, cts.Token);
        }

        private async UniTask ProcessEvents(IReadOnlyList<IGameStageEvent> events, CancellationToken cancellationToken)
        {
            statusRP.Value = GameStatus.StageDisplay;

            await eventEmitter.Emit(events, cancellationToken);

            var lastEvent = events[events.Count - 1];
            if (lastEvent is RoundEndedEvent)
            {
                statusRP.Value = GameStatus.None;
            }
            else
            {
                statusRP.Value = GameStatus.PlayerAction;
            }
        }
    }
}
