namespace MGSP.TrackPiece.App.Configs
{
    public static class GameUIConfig
    {
        // Game Result
        public static string PlayerWhiteWinMessage => "<sprite name=piece_white> Player Wins!";
        public static string PlayerBlackWinMessage => "<sprite name=piece_black> Player Wins!";
        public static string DrawMessage => "It's a draw!";

        // Board Size
        public static string ConfirmRestartWith4x4Message => "Restart with \r\na new 4x4 board?";
        public static string ConfirmRestartWith6x6Message => "Restart with \r\na new 6x6 board?";
    }
}
