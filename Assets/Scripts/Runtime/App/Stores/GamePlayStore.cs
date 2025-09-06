using MGSP.TrackPiece.Services;
using R3;
using System.Collections.Generic;
using System.Threading;
using VContainer;

namespace MGSP.TrackPiece.Stores
{
    public sealed class GamePlayStore
    {
        private readonly IGameService gameService;

        private readonly ReactiveProperty<bool> isInteractableRP = new(true);
        private readonly Queue<IGameStageEvent> eventQueue = new();
        private CancellationTokenSource cts = new();

        public ReadOnlyReactiveProperty<bool> IsInteractableRP => isInteractableRP;
        public CancellationToken CancellationToken => cts.Token;

        [Inject]
        public GamePlayStore(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public void SetInteractable(bool interactable)
        {
            isInteractableRP.Value = interactable;
        }

        public void CreateNewGame(GameLevel level)
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();

            var events = gameService.CreateNewGame(level);
            AddEvents(events);
        }

        public void Place(int positionIndex)
        {
            var events = gameService.Place(positionIndex);
            AddEvents(events);
        }

        public IGameStageEvent ReadEvent()
        {
            if (eventQueue.Count > 0)
            {
                return eventQueue.Dequeue();
            }
            else
            {
                return null;
            }
        }

        public void ClearAllEvents()
        {
            eventQueue.Clear();
        }

        private void AddEvents(IList<IGameStageEvent> events)
        {
            for (int i = 0; i < events.Count; i++)
            {
                eventQueue.Enqueue(events[i]);
            }
        }
    }
}
