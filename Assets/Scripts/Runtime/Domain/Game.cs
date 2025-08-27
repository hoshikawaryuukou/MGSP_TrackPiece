using System;

namespace MGSP.TrackPiece.Domain
{
    public enum PlayerId
    {
        None = 0,
        Player1 = 1,
        Player2 = 2
    }

    public enum GameResult
    {
        None,
        Player1Win,
        Player2Win,
        DoubleWin,
        Draw
    }

    public sealed class Game
    {
        private readonly BoardShifter boardShifter;
        private readonly WinChecker winChecker;
        private readonly PiecePlacer piecePlacer;
        private PlayerId[] board;
        private PlayerId activePlayerId;
        private GameResult result;

        private Game(GameConfig config, PlayerId[] initialBoard, PlayerId startingPlayer)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (initialBoard == null)
                throw new ArgumentNullException(nameof(initialBoard));

            if (initialBoard.Length != config.BoardSize)
                throw new ArgumentException($"Board must have exactly {config.BoardSize} positions.", nameof(initialBoard));

            if (startingPlayer == PlayerId.None)
                throw new ArgumentException("Starting player cannot be None.", nameof(startingPlayer));

            boardShifter = new BoardShifter(config.Track);
            winChecker = new WinChecker(config.WinningLines, config.LevelSize);
            piecePlacer = new PiecePlacer(config.BoardSize);

            board = initialBoard;
            activePlayerId = startingPlayer;
            result = GameResult.None;
        }

        public static Game CreateNew(GameConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var emptyBoard = new PlayerId[config.BoardSize];
            return new Game(config, emptyBoard, PlayerId.Player1);
        }

        public static Game CreateCustom(GameConfig config, PlayerId[] initialBoard, PlayerId startingPlayer)
        {
            return new Game(config, initialBoard, startingPlayer);
        }

        public PlayerId[] GetBoard()
        {
            return (PlayerId[])board.Clone();
        }

        public PlayerId GetActivePlayerId()
        {
            return activePlayerId;
        }

        public GameResult GetResult()
        {
            return result;
        }

        public void Place(int positionIndex)
        {
            piecePlacer.Place(board, positionIndex, activePlayerId);
        }

        public void Shift()
        {
            board = boardShifter.Shift(board);
        }

        public void Evaluate()
        {
            result = winChecker.CheckWin(board);
        }

        public void SwitchPlayer()
        {
            activePlayerId = activePlayerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
        }
    }
}
