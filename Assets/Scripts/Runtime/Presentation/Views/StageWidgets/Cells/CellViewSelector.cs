using R3;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.Views.StageWidgets
{
    public sealed class CellViewSelector : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask cellLayerMask = -1;

        public Observable<CellView> CellViewSelected =>
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0) && enabled)
                .Select(_ => GetClickedCell())
                .Where(cellView => cellView != null);

        void Awake()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        public void SetInteractable(bool value)
        {
            enabled = value;
        }

        private CellView GetClickedCell()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, cellLayerMask);

            var collider = hit.collider;
            if (collider != null)
            {
                return collider.GetComponent<CellView>();
            }

            return null;
        }
    }
}

