using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGSP.TrackPiece.Presentation.Views.UIWidgets
{
    public sealed class GameMenuView : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button boardSizeButton;
        [SerializeField] private TMP_Text boardSizeText;

        public Observable<Unit> RestartRequested => restartButton.OnClickAsObservable();
        public Observable<Unit> InfoRequested => infoButton.OnClickAsObservable();
        public Observable<Unit> BoardSizeChangeRequested => boardSizeButton.OnClickAsObservable();

        public void SetBoardSizeText(string message)
        {
            boardSizeText.SetText(message);
        }
    }
}
