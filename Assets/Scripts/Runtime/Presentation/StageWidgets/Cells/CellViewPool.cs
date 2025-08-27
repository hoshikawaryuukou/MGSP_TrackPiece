using UnityEngine;
using UnityEngine.Pool;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public sealed class CellViewPool : MonoBehaviour
    {
        [SerializeField] private CellView cellViewPrefab;
        [SerializeField] private Transform cellViewRoot;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 50;

        private ObjectPool<CellView> pool;

        private void Awake()
        {
            pool = new ObjectPool<CellView>(
                createFunc: CreateCellView,
                actionOnGet: OnGetCellView,
                actionOnRelease: OnReleaseCellView,
                actionOnDestroy: OnDestroyCellView,
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public CellView Get()
        {
            return pool.Get();
        }

        public void Release(CellView view)
        {
            if (view != null)
            {
                pool.Release(view);
            }
        }

        private CellView CreateCellView()
        {
            return Instantiate(cellViewPrefab, cellViewRoot);
        }

        private void OnGetCellView(CellView view)
        {
            view.gameObject.SetActive(true);
        }

        private void OnReleaseCellView(CellView view)
        {
            view.gameObject.SetActive(false);
        }

        private void OnDestroyCellView(CellView view)
        {
            if (view != null)
            {
                Destroy(view.gameObject);
            }
        }

        private void OnDestroy()
        {
            pool?.Dispose();
        }
    }
}
