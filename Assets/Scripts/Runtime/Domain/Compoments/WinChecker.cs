using System;

namespace MGSP.TrackPiece.Domain
{
    public sealed class WinChecker
    {
        private readonly int[][] winningLines;
        private readonly int lineLength;

        public WinChecker(int[][] winningLines, int lineLength)
        {
            this.winningLines = winningLines ?? throw new ArgumentNullException(nameof(winningLines));
            this.lineLength = lineLength;
        }

        public GameResult CheckWin(PlayerId[] board)
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
                return GameResult.DoubleWin;
            else if (p1Win)
                return GameResult.Player1Win;
            else if (p2Win)
                return GameResult.Player2Win;
            else if (IsBoardFull(board))
                return GameResult.Draw;
            else
                return GameResult.None;
        }

        private bool IsLineComplete(PlayerId[] board, int[] line, PlayerId playerId)
        {
            for (int i = 1; i < lineLength; i++)
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
