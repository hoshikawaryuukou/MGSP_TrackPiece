namespace MGSP.TrackPiece.Services
{
    public sealed class GameLevelConfig
    {
        public int BoardSideLength { get; }
        public int[] Track { get; }
        public int[][] WinningLines { get; }

        public GameLevelConfig(int boardSideLength, int[] track, int[][] winningLines)
        {
            BoardSideLength = boardSideLength;
            Track = track;
            WinningLines = winningLines;
        }
    }
}
