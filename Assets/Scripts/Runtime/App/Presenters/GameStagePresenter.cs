using Cysharp.Threading.Tasks;
using MGSP.TrackPiece.App.EventHandlers;
using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.App.Stores;
using R3;
using System;
using VContainer;
using VContainer.Unity;

namespace MGSP.TrackPiece.App.Presenters
{
    public sealed class GameStagePresenter : IInitializable, IDisposable
    {
        private readonly GamePlayStore gameStore;
        private readonly GameStartedEventHandler gameStartedEventHandler;
        private readonly GameEndedEventHandler gameEndedEventHandler;
        private readonly InputRequestedEventHandler inputRequestedEventHandler;
        private readonly InputInvalidEventHandler inputInvalidEventHandler;
        private readonly PiecePlacedEventHandler piecePlacedEventHandler;
        private readonly PiecesShiftedEventHandler piecesShiftedEventHandler;

        private readonly CompositeDisposable disposables = new();

        [Inject]
        public GameStagePresenter(GamePlayStore gameStore, GameStartedEventHandler gameStartedEventHandler, GameEndedEventHandler gameEndedEventHandler, InputRequestedEventHandler inputRequestedEventHandler, InputInvalidEventHandler inputInvalidEventHandler, PiecePlacedEventHandler piecePlacedEventHandler, PiecesShiftedEventHandler piecesShiftedEventHandler)
        {
            this.gameStore = gameStore;
            this.gameStartedEventHandler = gameStartedEventHandler;
            this.gameEndedEventHandler = gameEndedEventHandler;
            this.inputRequestedEventHandler = inputRequestedEventHandler;
            this.inputInvalidEventHandler = inputInvalidEventHandler;
            this.piecePlacedEventHandler = piecePlacedEventHandler;
            this.piecesShiftedEventHandler = piecesShiftedEventHandler;
        }

        void IInitializable.Initialize()
        {
            gameStore.IsDirtyRP
                .Where(isDirty => isDirty == true)
                .Subscribe(_ => Run().Forget())
                .AddTo(disposables);
        }

        void IDisposable.Dispose()
        {
            disposables.Dispose();
        }

        private async UniTask Run()
        {
            var events = gameStore.DequeueAllEvents();

            for (int i = 0; i < events.Count; i++)
            {
                var evt = events[i];

                switch (evt)
                {
                    case GameStartedEvent e:
                        gameStartedEventHandler.Handle(e);
                        break;
                    case GameEndedEvent e:
                        gameEndedEventHandler.Handle(e);
                        break;
                    case InputRequestedEvent e:
                        inputRequestedEventHandler.Handle(e);
                        break;
                    case InputInvalidEvent e:
                        inputInvalidEventHandler.Handle(e);
                        break;
                    case PiecePlacedEvent e:
                        piecePlacedEventHandler.Handle(e);
                        break;
                    case PiecesShiftedEvent e:
                        await piecesShiftedEventHandler.Handle(e);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                await UniTask.Yield();
            }
        }
    }
}
