using Cysharp.Threading.Tasks;
using MessagePipe;
using MGSP.TrackPiece.Presentation.Stores;
using MGSP.TrackPiece.Presentation.Views.StageWidgets;
using R3;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.Presentation.Presenters
{
    public sealed class GamePlayStatusPresenter : IInitializable, IDisposable
    {
        private readonly GamePlayStore gamePlayStore;
        private readonly GameUIStateStore gameUIStateStore;
        private readonly CellViewSelector cellViewSelector;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GamePlayStatusPresenter(GamePlayStore gamePlayStore, GameUIStateStore gameUIStateStore, CellViewSelector cellViewSelector)
        {
            this.gamePlayStore = gamePlayStore;
            this.gameUIStateStore = gameUIStateStore;
            this.cellViewSelector = cellViewSelector;
        }

        void IInitializable.Initialize()
        {
            gamePlayStore.StatusRP
                .Subscribe(status => Debug.Log($"GameStatus changed: {status}"))
                .AddTo(disposables);

            gamePlayStore.StatusRP
                .Select(status => status == GameStatus.PlayerAction)
                .Subscribe(cellViewSelector.SetInteractable)
                .AddTo(disposables);

            cellViewSelector.CellViewSelected      
                .Where(cellView => gameUIStateStore.IsInputBlocked.CurrentValue == false)
                .Where(cellView => cellView.isInteractable)
                .Select(cellView => cellView.positionIndex)
                .Subscribe(positionIndex => gamePlayStore.Place(positionIndex).Forget())
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }
    }
}
