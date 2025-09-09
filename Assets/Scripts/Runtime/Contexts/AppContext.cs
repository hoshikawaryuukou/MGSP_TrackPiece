using MessagePipe;
using MGSP.TrackPiece.App.Presenters;
using MGSP.TrackPiece.App.Stores;
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
            builder.RegisterMessagePipe();

            builder.RegisterInstance<IGameService>(new StandaloneGameService());

            builder.RegisterInstance(new GameUIStateStore());
            builder.Register<GamePlayStore>(Lifetime.Singleton);
            builder.Register<GameStageEventEmitter>(Lifetime.Singleton);
 
            builder.RegisterComponentInHierarchy<GameInfoView>();
            builder.RegisterComponentInHierarchy<GameMenuView>();
            builder.RegisterComponentInHierarchy<GameResultView>();
            builder.RegisterComponentInHierarchy<ConfirmDialogView>();
            builder.RegisterComponentInHierarchy<GameStageView>();
            builder.RegisterComponentInHierarchy<CellViewSelector>();

            builder.RegisterEntryPoint<AppPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameMenuLevelChangePresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameMenuRestartPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameMenuInfoPresenter>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GamePlayStatusPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GamePlayRoundPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GamePlayTrunPresenter>(Lifetime.Singleton);
        }
    }
}
