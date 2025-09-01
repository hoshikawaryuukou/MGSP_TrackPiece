using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGSP.TrackPiece.Presentation.UIWidgets
{
    public sealed class ConfirmDialogView : MonoBehaviour
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private TMP_Text messageText;

        public Observable<Unit> YesRequested => yesButton.OnClickAsObservable();
        public Observable<Unit> NoRequested => noButton.OnClickAsObservable();

        public void SetMessage(string message)
        {
            messageText.SetText(message);
        }

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
