using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Stores;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class AppPresenter : IPostStartable 
    {
        private readonly GamePlayStore gamePlayStore;

        [Inject]
        public AppPresenter(GamePlayStore gamePlayStore)
        {
            this.gamePlayStore = gamePlayStore;
        }

        void IPostStartable.PostStart()
        {
            gamePlayStore.CreateNewGame(GameLevel._4x4);
        }
    }
}
