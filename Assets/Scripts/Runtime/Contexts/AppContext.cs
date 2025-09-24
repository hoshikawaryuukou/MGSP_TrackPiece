using MessagePipe;
using MGSP.TrackPiece.Presentation.Presenters;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.StageWidgets;
using MGSP.TrackPiece.Presentation.Views.UIWidgets;
using MGSP.TrackPiece.Services;
using MGSP.TrackPiece.Services.Events;
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
