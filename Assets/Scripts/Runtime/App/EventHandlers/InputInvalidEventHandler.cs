using MGSP.TrackPiece.App.Events;
using UnityEngine;

namespace MGSP.TrackPiece.App.EventHandlers
{
    public sealed class InputInvalidEventHandler
    {
        public void Handle(InputInvalidEvent evt)
        {
            Debug.Log($"InvalidMove: {evt.Reason}");
        }
    }
}
