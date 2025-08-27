using System;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Domain
{
    public sealed class BoardShifter
    {
        private readonly IReadOnlyList<int> track;
        private readonly int boardSize;

        public BoardShifter(IReadOnlyList<int> track)
        {
            this.track = track ?? throw new ArgumentNullException(nameof(track));
            boardSize = track.Count;
        }

        public PlayerId[] Shift(PlayerId[] currentBoard)
        {
            if (currentBoard == null)
                throw new ArgumentNullException(nameof(currentBoard));

            if (currentBoard.Length != boardSize)
                throw new ArgumentException($"Board must have exactly {boardSize} positions.", nameof(currentBoard));

            var newBoard = new PlayerId[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                newBoard[track[i]] = currentBoard[i];
            }

            return newBoard;
        }
    }
}
