using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Domain;
using R3;
using System.Collections.Generic;

namespace MGSP.TrackPiece.App.Stores
{
    public sealed class GamePlayStore
    {
        private readonly ReactiveProperty<bool> isDirtyRP = new(false);
        private readonly Subject<Unit> cancelTriggered = new();
        private readonly Queue<IGameStageEvent> eventQueue = new();
        private GameLevel currentLevel = GameLevel._4x4;
        private Game game = null;

        public ReadOnlyReactiveProperty<bool> IsDirtyRP => isDirtyRP;
        public Observable<Unit> CancelTriggered => cancelTriggered;

        public void CreateNewGame(GameLevel level)
        {
            cancelTriggered.OnNext(Unit.Default);

            currentLevel = level;
            game = level switch
            {
                GameLevel._4x4 => GameBuilder.Create4x4Game(),
                GameLevel._6x6 => GameBuilder.Create6x6Game(),
                _ => throw new System.InvalidOperationException("Unsupported board size."),
            };

            eventQueue.Enqueue(new GameStartedEvent(level, game.GetActivePlayerId()));
            eventQueue.Enqueue(new InputRequestedEvent(game.GetActivePlayerId()));
            NotifyEventsAvailable();
        }

        public void Place(int positionIndex)
        {
            try
            {
                game.Place(positionIndex);
                eventQueue.Enqueue(new PiecePlacedEvent(game.GetActivePlayerId(), positionIndex));

                game.Shift();
                eventQueue.Enqueue(new PiecesShiftedEvent(currentLevel));

                game.Evaluate();

                var result = game.GetResult();
                if (result == GameResult.None)
                {
                    game.SwitchPlayer();
                    eventQueue.Enqueue(new InputRequestedEvent(game.GetActivePlayerId()));
                }
                else
                {
                    eventQueue.Enqueue(new GameEndedEvent(result));
                }
            }
            catch (System.Exception ex)
            {
                eventQueue.Enqueue(new InputInvalidEvent(ex.Message, positionIndex));
                eventQueue.Enqueue(new InputRequestedEvent(game.GetActivePlayerId()));
            }

            NotifyEventsAvailable();
        }

        public IList<IGameStageEvent> DequeueAllEvents()
        {
            var events = new List<IGameStageEvent>();

            while (eventQueue.Count > 0)
            {
                events.Add(eventQueue.Dequeue());
            }

            if (events.Count > 0)
            {
                isDirtyRP.Value = false;
            }

            return events;
        }

        public void ClearAllEvents()
        {
            eventQueue.Clear();
            isDirtyRP.Value = false;
        }

        private void NotifyEventsAvailable()
        {
            if (eventQueue.Count > 0)
            {
                isDirtyRP.Value = true;
            }
        }
    }
}
