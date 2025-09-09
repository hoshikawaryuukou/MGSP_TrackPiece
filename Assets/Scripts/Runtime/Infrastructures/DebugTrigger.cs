using Alchemy.Inspector;
using UnityEngine;

namespace MGSP.TrackPiece.Infrastructures
{
    public sealed class DebugTrigger : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnValueChanged")] 
        private bool enable;

        void Awake()
        {
            Debug.unityLogger.logEnabled = enable;
        }

        private void OnValueChanged()
        {
            Debug.unityLogger.logEnabled = enable;
        }
    }
}
