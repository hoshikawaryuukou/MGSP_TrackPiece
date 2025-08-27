using UnityEngine;
using UnityEngine.Pool;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public enum PieceType { WHITE, BLACK }

    public sealed class PieceViewPool : MonoBehaviour
    {
        [SerializeField] private PieceView pieceViewPrefab;
        [SerializeField] private Transform pieceViewRoot;
        [SerializeField] private Sprite whitePieceSprite;
        [SerializeField] private Sprite blackPieceSprite;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 50;

        private ObjectPool<PieceView> pool;

        private void Awake()
        {
            pool = new ObjectPool<PieceView>(
                createFunc: CreatePieceView,
                actionOnGet: OnGetPieceView,
                actionOnRelease: OnReleasePieceView,
                actionOnDestroy: OnDestroyPieceView,
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }

        public PieceView Get(PieceType pieceType)
        {
            var view = pool.Get();
            SetPieceType(view, pieceType);
            return view;
        }

        public void Release(PieceView view)
        {
            if (view != null)
            {
                pool.Release(view);
            }
        }

        private PieceView CreatePieceView()
        {
            return Instantiate(pieceViewPrefab, pieceViewRoot);
        }

        private void OnGetPieceView(PieceView view)
        {
            view.gameObject.SetActive(true);
        }

        private void OnReleasePieceView(PieceView view)
        {
            view.gameObject.SetActive(false);
        }

        private void OnDestroyPieceView(PieceView view)
        {
            if (view != null)
            {
                Destroy(view.gameObject);
            }
        }

        private void SetPieceType(PieceView view, PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.WHITE:
                    view.SetSprite(whitePieceSprite);
                    break;
                case PieceType.BLACK:
                    view.SetSprite(blackPieceSprite);
                    break;
            }
        }

        private void OnDestroy()
        {
            pool?.Dispose();
        }
    }
}
