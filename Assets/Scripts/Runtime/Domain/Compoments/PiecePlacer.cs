using System;

namespace MGSP.TrackPiece.Domain
{
    public sealed class PiecePlacer
    {
        private readonly int boardSize;

        public PiecePlacer(int boardSize)
        {
            if (boardSize <= 0)
                throw new ArgumentException("Board size must be positive.", nameof(boardSize));

            this.boardSize = boardSize;
        }

        public void Place(PlayerId[] board, int positionIndex, PlayerId playerId)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            if (board.Length != boardSize)
                throw new ArgumentException($"Board must have exactly {boardSize} positions.", nameof(board));

            if (positionIndex < 0 || positionIndex >= boardSize)
                throw new ArgumentOutOfRangeException(nameof(positionIndex));

            if (board[positionIndex] != PlayerId.None)
                throw new InvalidOperationException("Position already occupied.");

            if (playerId == PlayerId.None)
                throw new ArgumentException("Cannot place None player.", nameof(playerId));

            board[positionIndex] = playerId;
        }
    }
}
