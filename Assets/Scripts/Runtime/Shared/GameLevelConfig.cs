namespace MGSP.TrackPiece.Shared
{
    public class GameLevelConfig
    {
        public int BoardSizeLength { get; }
        public int[] Track { get; }
        public int[][] WinningLines { get; }

        public GameLevelConfig(int boardSizeLength, int[] track, int[][] winningLines)
        {
            BoardSizeLength = boardSizeLength;
            Track = track;
            WinningLines = winningLines;
        }
    }
}
