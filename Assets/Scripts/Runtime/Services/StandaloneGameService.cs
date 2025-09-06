using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Shared;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Services
{
    public sealed class StandaloneGameService : IGameService
    {
        private Game game = null;

        public IList<IGameStageEvent> CreateNewGame(GameLevel level)
        {
            var levelConfig = level switch
            {
                GameLevel._4x4 => GameLevelConfigTable.Config4x4,
                GameLevel._6x6 => GameLevelConfigTable.Config6x6,
                _ => throw new System.InvalidOperationException("Unsupported board size."),
            };

            var gameConfig = new GameConfig(levelConfig.BoardSizeLength, levelConfig.Track, levelConfig.WinningLines);
            game = Game.CreateNew(gameConfig);

            var events = new List<IGameStageEvent>();
            events.Add(new GameStartedEvent(level));
            events.Add(new TurnStartedEvent(GetActiveGamePlayer(game), game.GetInteractablePositions()));
            return events;
        }

        public IList<IGameStageEvent> Place(int positionIndex)
        {
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
                events.Add(new GameEndedEvent(ConvertResult(status)));
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
