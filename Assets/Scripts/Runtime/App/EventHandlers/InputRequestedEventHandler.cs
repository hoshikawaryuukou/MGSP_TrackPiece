using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.App.Stores;
using MGSP.TrackPiece.Presentation.StageWidgets.Cells;
using R3;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class InputRequestedEventHandler
    {
        private readonly GameUIStore gameUIStore;
        private readonly GamePlayStore gameStore;
        private readonly CellViewSelector cellViewSelector;

        [Inject]
        public InputRequestedEventHandler(GameUIStore gameUIStore, GamePlayStore gameStore, CellViewSelector cellViewSelector)
        {
            this.gameUIStore = gameUIStore;
            this.gameStore = gameStore;
            this.cellViewSelector = cellViewSelector;
        }

        public void Handle(InputRequestedEvent evt)
        {
            var disposables = new CompositeDisposable();
            cellViewSelector.CellViewSelected
                .Where(_ => gameUIStore.IsInteractableRP.CurrentValue)
                .Select(cellView => cellView.positionIndex)
                .TakeUntil(gameStore.CancelTriggered)
                .Subscribe(positionIndex =>
                {
                    gameStore.Place(positionIndex);
                    disposables.Dispose();
                })
                .AddTo(disposables);
        }
    }
}
