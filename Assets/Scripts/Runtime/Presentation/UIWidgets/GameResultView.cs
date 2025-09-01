using TMPro;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.UIWidgets
{
    public sealed class GameResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        public void Show(string message)
        {
            messageText.SetText(message);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
