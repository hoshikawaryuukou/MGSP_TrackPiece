using R3;
using System.Collections.Generic;

namespace MGSP.TrackPiece.App.Stores
{
    public interface IInputBlocker { }

    public sealed class GameUIStateStore
    {
        private readonly HashSet<IInputBlocker> inputBlockers = new();
        private readonly ReactiveProperty<bool> isInputBlocked = new(false);

        public ReadOnlyReactiveProperty<bool> IsInputBlocked => isInputBlocked;

        public void RequestInputBlock(IInputBlocker requester)
        {
            if (inputBlockers.Add(requester))
            {
                if (inputBlockers.Count == 1)
                {
                    isInputBlocked.Value = true;
                }
            }
        }

        public void ReleaseInputBlock(IInputBlocker requester)
        {
            if (inputBlockers.Remove(requester))
            {
                if (inputBlockers.Count == 0)
                {
                    isInputBlocked.Value = false;
                }
            }
        }
    }
}