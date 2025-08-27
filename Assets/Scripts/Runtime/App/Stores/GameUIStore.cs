using MGSP.TrackPiece.Domain;
using R3;

namespace MGSP.TrackPiece.App.Stores
{
    public sealed class GameUIStore
    {
        private readonly ReactiveProperty<GameLevel> levelRP = new(GameLevel._4x4);
        private readonly ReactiveProperty<bool> isInteractableRP = new(true);

        public ReadOnlyReactiveProperty<GameLevel> LevelRP => levelRP;
        public ReadOnlyReactiveProperty<bool> IsInteractableRP => isInteractableRP;

        public void SetLevel(GameLevel level)
        {
            levelRP.Value = level;
        }

        public void SetInteractable(bool interactable)
        {
            isInteractableRP.Value = interactable;
        }
    }
}
