using R3;

namespace MGSP.TrackPiece.Stores
{
    public sealed class GameMenuStore
    {
        private readonly ReactiveProperty<bool> infoOnRP = new(false);
        private readonly Subject<bool> levelChangeRequested = new();

        public ReadOnlyReactiveProperty<bool> InfoOnRP => infoOnRP;
        public Observable<bool> LevelChangeRequested => levelChangeRequested;

        public void SetInfoOn(bool isOn)
        {
            infoOnRP.Value = isOn;
        }

        public void RequestLevelChange()
        {
            levelChangeRequested.OnNext(true);
        }
    }
}
