namespace MGSP.TrackPiece.Domain
{
    public sealed class GameConfig
    {
        public int BoardSize { get; }
        public int[] Track { get; }
        public int[][] WinningLines { get; }

        public GameConfig(int boardSideLength, int[] track, int[][] winningLines)
        {
            BoardSize = boardSideLength * boardSideLength;
            Track = track;
            WinningLines = winningLines;
        }
    }
}
