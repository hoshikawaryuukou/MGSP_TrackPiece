namespace MGSP.TrackPiece.Domain
{
    public static class GameBuilder
    {
        public static Game Create4x4Game() => Game.CreateNew(GameConfigTable.GetConfig(GameLevel._4x4));
        public static Game Create6x6Game() => Game.CreateNew(GameConfigTable.GetConfig(GameLevel._6x6));
        public static Game CreateCustomGame(GameConfig gameConfig, PlayerId[] initialBoard, PlayerId startingPlayer) => Game.CreateCustom(gameConfig, initialBoard, startingPlayer);
    }
}
