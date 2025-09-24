using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Services.Events;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Services
{
    public sealed class StandaloneGameService : IGameService
    {
        private Game game = null;

        public async UniTask<IReadOnlyList<IGameStageEvent>> CreateNewGame(GameLevel level)
        {
            // fake async operation
            await UniTask.Yield();

            var levelConfig = GameLevelConfigTable.Table[level];
            var gameConfig = new GameConfig(levelConfig.BoardSideLength, levelConfig.Track, levelConfig.WinningLines);
            game = Game.CreateNew(gameConfig);

            return new List<IGameStageEvent>
            {
                new RoundStartedEvent(level),
                new TurnStartedEvent(GetActiveGamePlayer(game), game.GetInteractablePositions())
            };
        }

        public async UniTask<IReadOnlyList<IGameStageEvent>> PlacePiece(int positionIndex)
        {
            // fake async operation
            await UniTask.Yield();

            var events = new List<IGameStageEvent>();

            game.Place(positionIndex);
            game.Shift();
            events.Add(new TurnEndedEvent(GetActiveGamePlayer(game), positionIndex));

            game.Evaluate();

            var status = game.GetStatus();
            if (status == GameStatus.None)
            {
                game.SwitchPlayer();
                events.Add(new TurnStartedEvent(GetActiveGamePlayer(game), game.GetInteractablePositions()));
            }
            else
            {
                events.Add(new RoundEndedEvent(ConvertResult(status)));
                game = null;
            }

            return events;
        }

        private GamePlayer GetActiveGamePlayer(Game game)
        {
            return game.GetActivePlayerId() switch
            {
                PlayerId.Player1 => GamePlayer.White,
                PlayerId.Player2 => GamePlayer.Black,
                _ => throw new System.InvalidOperationException("Invalid player ID."),
            };
        }

        private GameResult ConvertResult(GameStatus result)
        {
            return result switch
            {
                GameStatus.Player1Win => GameResult.PlayerWhite,
                GameStatus.Player2Win => GameResult.PlayerBlack,
                GameStatus.Draw => GameResult.Draw,
                GameStatus.DoubleWin => GameResult.Draw,
                _ => throw new System.InvalidOperationException("Invalid game result."),
            };
        }
    }
}
