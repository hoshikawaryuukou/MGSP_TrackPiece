using UnityEngine;
using R3;
using R3.Triggers;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public int positionIndex;

        public Observable<Unit> Clicked => spriteRenderer.OnMouseDownAsObservable();
    }
}
