namespace MGSP.TrackPiece.Shared
{
    public static class GameLevelConfigTable
    {
        public static readonly GameLevelConfig Config4x4 = Create4x4();
        public static readonly GameLevelConfig Config6x6 = Create6x6();

        private static GameLevelConfig Create4x4()
        {
            var track = new int[] { 4, 0, 1, 2, 8, 9, 5, 3, 12, 10, 6, 7, 13, 14, 15, 11 };
            var winningLines = new int[][]
            {
                // Horizontal (4 lines)
                new int[] { 0, 1, 2, 3 },      // Row 1
                new int[] { 4, 5, 6, 7 },      // Row 2
                new int[] { 8, 9, 10, 11 },    // Row 3
                new int[] { 12, 13, 14, 15 },  // Row 4
                
                // Vertical (4 lines)
                new int[] { 0, 4, 8, 12 },     // Column 1
                new int[] { 1, 5, 9, 13 },     // Column 2
                new int[] { 2, 6, 10, 14 },    // Column 3
                new int[] { 3, 7, 11, 15 },    // Column 4
                
                // Diagonal (2 lines)
                new int[] { 0, 5, 10, 15 },    // Main diagonal
                new int[] { 3, 6, 9, 12 }      // Anti-diagonal
            };

            return new GameLevelConfig(4, track, winningLines);
        }

        private static GameLevelConfig Create6x6()
        {
            var track = new int[] { 6, 0, 1, 2, 3, 4, 12, 13, 7, 8, 9, 5, 18, 19, 20, 14, 10, 11, 24, 25, 21, 15, 16, 17, 30, 26, 27, 28, 22, 23, 31, 32, 33, 34, 35, 29 };

            var winningLines = new int[][]
            {
                // Horizontal (6 lines)
                new int[] { 0, 1, 2, 3, 4, 5 },      // Row 1
                new int[] { 6, 7, 8, 9, 10, 11 },    // Row 2
                new int[] { 12, 13, 14, 15, 16, 17 },// Row 3
                new int[] { 18, 19, 20, 21, 22, 23 },// Row 4
                new int[] { 24, 25, 26, 27, 28, 29 },// Row 5
                new int[] { 30, 31, 32, 33, 34, 35 },// Row 6

                // Vertical (6 lines)
                new int[] { 0, 6, 12, 18, 24, 30 },  // Column 1
                new int[] { 1, 7, 13, 19, 25, 31 },  // Column 2
                new int[] { 2, 8, 14, 20, 26, 32 },  // Column 3
                new int[] { 3, 9, 15, 21, 27, 33 },  // Column 4
                new int[] { 4, 10, 16, 22, 28, 34 }, // Column 5
                new int[] { 5, 11, 17, 23, 29, 35 }, // Column 6

                // Diagonal (2 lines)
                new int[] { 0, 7, 14, 21, 28, 35 },  // Main diagonal
                new int[] { 5, 10, 15, 20, 25, 30 }, // Anti-diagonal
            };

            return new GameLevelConfig(6, track, winningLines);
        }
    }
}
