using MGSP.TrackPiece.App.Presenters;
using MGSP.TrackPiece.Presentation.StageWidgets;
using MGSP.TrackPiece.Presentation.UIWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Stores;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Contexts
{
    public sealed class AppContext : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<IGameService>(new StandaloneGameService());

            builder.Register<GamePlayStore>(Lifetime.Singleton);
            builder.RegisterInstance(new GameMenuStore());
            builder.RegisterInstance(new GameOptionStore());

            builder.RegisterComponentInHierarchy<GameInfoView>();
            builder.RegisterComponentInHierarchy<GameMenuView>();
            builder.RegisterComponentInHierarchy<GameResultView>();
            builder.RegisterComponentInHierarchy<ConfirmDialogView>();
            builder.RegisterComponentInHierarchy<GameStageView>();
            builder.RegisterComponentInHierarchy<CellViewSelector>();

            builder.RegisterEntryPoint<AppPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameRestartPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GamePlayPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameInfoPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameLevelChangePresenter>(Lifetime.Singleton);
        }
    }
}
