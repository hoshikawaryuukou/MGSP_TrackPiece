using System;

namespace MGSP.TrackPiece.Domain.Compoments
{
    public sealed class PiecePlacer
    {
        public void Place(PlayerId[] board, int positionIndex, PlayerId playerId)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            var boardSize = board.Length;

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
