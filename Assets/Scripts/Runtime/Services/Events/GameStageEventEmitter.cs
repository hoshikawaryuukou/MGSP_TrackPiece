using Cysharp.Threading.Tasks;
using MessagePipe;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

namespace MGSP.TrackPiece.Services.Events
{
    public sealed class GameStageEventEmitter
    {
        private readonly IAsyncPublisher<RoundStartedEvent> roundStartedPublisher;
        private readonly IAsyncPublisher<RoundEndedEvent> roundEndedPublisher;
        private readonly IAsyncPublisher<TurnStartedEvent> turnStartedPublisher;
        private readonly IAsyncPublisher<TurnEndedEvent> turnEndedPublisher;

        [Inject]
        public GameStageEventEmitter(IAsyncPublisher<RoundStartedEvent> roundStartedPublisher, IAsyncPublisher<RoundEndedEvent> roundEndedPublisher, IAsyncPublisher<TurnStartedEvent> turnStartedPublisher, IAsyncPublisher<TurnEndedEvent> turnEndedPublisher)
        {
            this.roundStartedPublisher = roundStartedPublisher;
            this.roundEndedPublisher = roundEndedPublisher;
            this.turnStartedPublisher = turnStartedPublisher;
            this.turnEndedPublisher = turnEndedPublisher;
        }

        public async UniTask Emit(IReadOnlyList<IGameStageEvent> events, CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < events.Count; i++)
            {
                var e = events[i];
                Debug.Log($"[GameStageEventEmitter] Emit: {e.GetType().Name}");

                switch (e)
                {
                    case RoundStartedEvent roundStartedEvt: await roundStartedPublisher.PublishAsync(roundStartedEvt, cancellationToken); break;
                    case RoundEndedEvent roundEndedEvt: await roundEndedPublisher.PublishAsync(roundEndedEvt, cancellationToken); break;
                    case TurnStartedEvent turnStartedEvt: await turnStartedPublisher.PublishAsync(turnStartedEvt, cancellationToken); break;
                    case TurnEndedEvent turnEndedEvt: await turnEndedPublisher.PublishAsync(turnEndedEvt, cancellationToken); break;
                }
                await UniTask.Yield(cancellationToken);
            }
        }
    }
}
