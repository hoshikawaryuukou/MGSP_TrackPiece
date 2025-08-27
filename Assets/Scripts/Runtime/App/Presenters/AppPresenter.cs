using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Domain;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class AppPresenter : IPostStartable 
    {
        private readonly GamePlayStore gameStore;

        [Inject]
        public AppPresenter(GamePlayStore gameStore)
        {
            this.gameStore = gameStore;
        }

        void IPostStartable.PostStart()
        {
            gameStore.CreateNewGame(GameLevel._4x4);
        }
    }
}
