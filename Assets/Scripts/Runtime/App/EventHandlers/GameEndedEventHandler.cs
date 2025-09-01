using MGSP.TrackPiece.App.Configs;
using MGSP.TrackPiece.App.Events;
using MGSP.TrackPiece.Domain;
using MGSP.TrackPiece.Presentation.UIWidgets;
using System;
using VContainer;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class GameEndedEventHandler 
    {
        private readonly GameResultView gameResultView;

        [Inject]
        public GameEndedEventHandler(GameResultView gameResultView)
        {
            this.gameResultView = gameResultView;
        }

        public void Handle(GameEndedEvent evt)
        {
            var resultMessage = evt.Result switch
            {
                GameResult.Player1Win => GameUIConfig.PlayerWhiteWinMessage,
                GameResult.Player2Win => GameUIConfig.PlayerBlackWinMessage,
                GameResult.Draw => GameUIConfig.DrawMessage,
                GameResult.DoubleWin => GameUIConfig.DrawMessage,
                _ => throw new InvalidOperationException("Unexpected game result."),
            };

            gameResultView.Show(resultMessage);
        }
    }
}
