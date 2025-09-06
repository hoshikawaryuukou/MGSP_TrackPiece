using MGSP.TrackPiece.Domain.Compoments;
using System;

namespace MGSP.TrackPiece.Domain
{
    public enum PlayerId { None, Player1, Player2 }

    public enum GameStatus { None, Player1Win, Player2Win, DoubleWin, Draw }

    public sealed class Game
    {
        private readonly BoardShifter boardShifter;
        private readonly WinChecker winChecker;
        private readonly PiecePlacer piecePlacer;
        private PlayerId[] board;
        private PlayerId activePlayerId;
        private GameStatus status;

        private Game(GameConfig config, PlayerId[] initialBoard, PlayerId startingPlayer)
        {
            piecePlacer = new PiecePlacer();
            boardShifter = new BoardShifter(config.Track);
            winChecker = new WinChecker(config.WinningLines);

            board = initialBoard;
            activePlayerId = startingPlayer;
            status = GameStatus.None;
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
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (initialBoard == null)
                throw new ArgumentNullException(nameof(initialBoard));

            if (initialBoard.Length != config.BoardSize)
                throw new ArgumentException($"Board must have exactly {config.BoardSize} positions.", nameof(initialBoard));

            if (startingPlayer == PlayerId.None)
                throw new ArgumentException("Starting player cannot be None.", nameof(startingPlayer));

            return new Game(config, initialBoard, startingPlayer);
        }

        public PlayerId[] GetBoard()
        {
            return (PlayerId[])board.Clone();
        }

        public bool[] GetInteractablePositions()
        {
            var interactable = new bool[board.Length];
            for (int i = 0; i < board.Length; i++)
            {
                interactable[i] = board[i] == PlayerId.None;
            }
            return interactable;
        }

        public PlayerId GetActivePlayerId()
        {
            return activePlayerId;
        }

        public GameStatus GetStatus()
        {
            return status;
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
            status = winChecker.CheckWin(board);
        }

        public void SwitchPlayer()
        {
            activePlayerId = activePlayerId == PlayerId.Player1 ? PlayerId.Player2 : PlayerId.Player1;
        }
    }
}
