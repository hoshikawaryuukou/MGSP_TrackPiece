using R3;
using UnityEngine;
using UnityEngine.UI;

namespace MGSP.TrackPiece.Presentation.Views.UIWidgets
{
    public sealed class GameInfoView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        public Observable<Unit> CloseRequested => closeButton.OnClickAsObservable();

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
