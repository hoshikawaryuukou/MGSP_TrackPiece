using MGSP.TrackPiece.App.EventHandlers;
using MGSP.TrackPiece.App.Presenters;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Presentation.StageWidgets;
using MGSP.TrackPiece.Presentation.StageWidgets.Cells;
using MGSP.TrackPiece.Presentation.UIWidgets;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Contexts
{
    public sealed class AppContext : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameInfoView>();
            builder.RegisterComponentInHierarchy<GameMenuView>();
            builder.RegisterComponentInHierarchy<GameResultBannerView>();
            builder.RegisterComponentInHierarchy<ConfirmDialogModalView>();
            builder.RegisterComponentInHierarchy<GameStageView>();
            builder.RegisterComponentInHierarchy<CellViewSelector>();

            builder.RegisterInstance(new GamePlayStore());
            builder.RegisterInstance(new GameUIStore());

            builder.Register<GameStartedEventHandler>(Lifetime.Singleton);
            builder.Register<GameEndedEventHandler>(Lifetime.Singleton);
            builder.Register<InputRequestedEventHandler>(Lifetime.Singleton);
            builder.RegisterInstance(new InputInvalidEventHandler());
            builder.Register<PiecePlacedEventHandler>(Lifetime.Singleton);
            builder.Register<PiecesShiftedEventHandler>(Lifetime.Singleton);

            builder.RegisterEntryPoint<AppPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameMenuPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameStagePresenter>(Lifetime.Singleton);
        }
    }
}
