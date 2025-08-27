using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGSP.TrackPiece.Presentation.UIWidgets
{
    public sealed class GameResultBannerView : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text messageText;

        public void Show(string message)
        {
            backgroundImage.enabled = true;
            messageText.gameObject.SetActive(true); 
            messageText.SetText(message);
        }

        public void Hide()
        {
            backgroundImage.enabled = false;
            messageText.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        [BoxGroup("Editor Commands"), Button] private void _Show() => Show("Test Message!");
        [BoxGroup("Editor Commands"), Button] private void _Hide() => Hide();
#endif
    }
}
