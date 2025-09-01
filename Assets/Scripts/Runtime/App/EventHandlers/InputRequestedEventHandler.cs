using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Presentation.StageWidgets.Cells;
using R3;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class InputRequestedEventHandler
    {
        private readonly GameMenuStore gameMenuStore;
        private readonly GamePlayStore gamePlayStore;
        private readonly CellViewSelector cellViewSelector;

        [Inject]
        public InputRequestedEventHandler(GameMenuStore gameMenuStore, GamePlayStore gamePlayStore, CellViewSelector cellViewSelector)
        {
            this.gameMenuStore = gameMenuStore;
            this.gamePlayStore = gamePlayStore;
            this.cellViewSelector = cellViewSelector;
        }

        public void Handle(InputRequestedEvent _)
        {
            var disposables = new CompositeDisposable();
            cellViewSelector.CellViewSelected
                .Where(_ => gameMenuStore.IsInteractableRP.CurrentValue)
                .Select(cellView => cellView.positionIndex)
                .TakeUntil(gamePlayStore.CancelTriggered)
                .Subscribe(positionIndex =>
                {
                    gamePlayStore.Place(positionIndex);
                    disposables.Dispose();
                })
                .AddTo(disposables);
        }
    }
}
