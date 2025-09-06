using System;
using System.Collections.Generic;

namespace MGSP.TrackPiece.Domain.Compoments
{
    public sealed class BoardShifter
    {
        private readonly IReadOnlyList<int> track;

        public BoardShifter(IReadOnlyList<int> track)
        {
            this.track = track ?? throw new ArgumentNullException(nameof(track));
        }

        public PlayerId[] Shift(PlayerId[] currentBoard)
        {
            if (currentBoard == null)
                throw new ArgumentNullException(nameof(currentBoard));

            var boardSize = currentBoard.Length;

            if (boardSize != track.Count)
                throw new ArgumentException($"Board must have exactly {track.Count} positions to match the track.", nameof(currentBoard));

            var newBoard = new PlayerId[boardSize];

            for (int i = 0; i < boardSize; i++)
            {
                newBoard[track[i]] = currentBoard[i];
            }

            return newBoard;
        }
    }
}
