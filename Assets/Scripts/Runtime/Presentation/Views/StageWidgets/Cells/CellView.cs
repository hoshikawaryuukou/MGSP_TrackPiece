using UnityEngine;
using R3;
using R3.Triggers;

namespace MGSP.TrackPiece.Presentation.Views.StageWidgets
{
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public int positionIndex;
        public bool isInteractable = true;

        public Observable<Unit> Clicked => spriteRenderer.OnMouseDownAsObservable();

        public void SetInteractable(bool value)
        {
            isInteractable = value;
        }
    }
}
