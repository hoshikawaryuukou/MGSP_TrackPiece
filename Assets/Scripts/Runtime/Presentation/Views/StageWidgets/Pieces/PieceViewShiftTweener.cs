using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.Views.StageWidgets
{
    public sealed class PieceViewShiftTweener : MonoBehaviour
    {
        [SerializeField] private GridView gridView;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease ease = Ease.OutQuad;

        public UniTask Run(Transform item, Vector3 from, Vector3 to)
        {
            return LMotion.Create(from, to, duration)
                .WithEase(ease)
                .BindToPosition(item)
                .ToUniTask();
        }
    }
}
