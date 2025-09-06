using MGSP.TrackPiece.Services;
using R3;

namespace MGSP.TrackPiece.Stores
{
    public sealed class GameOptionStore
    {
        private readonly ReactiveProperty<GameLevel> levelRP = new(GameLevel._4x4);

        public ReadOnlyReactiveProperty<GameLevel> LevelRP => levelRP;

        public void SetLevel(GameLevel level)
        {
            levelRP.Value = level;
        }
    }
}
