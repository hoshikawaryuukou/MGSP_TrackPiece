using System;

namespace MGSP.TrackPiece.Domain.Compoments
{
    public sealed class WinChecker
    {
        private readonly int[][] winningLines;

        public WinChecker(int[][] winningLines)
        {
            this.winningLines = winningLines ?? throw new ArgumentNullException(nameof(winningLines));
        }

        public GameStatus CheckWin(PlayerId[] board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            bool p1Win = false, p2Win = false;

            for (int i = 0; i < winningLines.Length; i++)
            {
                var line = winningLines[i];
                var firstCell = board[line[0]];
                if (firstCell == PlayerId.None)
                    continue;

                if (IsLineComplete(board, line, firstCell))
                {
                    if (firstCell == PlayerId.Player1)
                        p1Win = true;
                    else if (firstCell == PlayerId.Player2)
                        p2Win = true;

                    if (p1Win && p2Win) break;
                }
            }

            if (p1Win && p2Win)
                return GameStatus.DoubleWin;
            else if (p1Win)
                return GameStatus.Player1Win;
            else if (p2Win)
                return GameStatus.Player2Win;
            else if (IsBoardFull(board))
                return GameStatus.Draw;
            else
                return GameStatus.None;
        }

        private bool IsLineComplete(PlayerId[] board, int[] line, PlayerId playerId)
        {
            for (int i = 1; i < line.Length; i++)
            {
                if (board[line[i]] != playerId)
                    return false;
            }
            return true;
        }

        private bool IsBoardFull(PlayerId[] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == PlayerId.None)
                    return false;
            }
            return true;
        }
    }
}
