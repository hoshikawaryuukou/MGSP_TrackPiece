using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Services;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
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
            gamePlayStore.CreateNewGame(GameLevel._4x4).Forget();
        }
    }
}
