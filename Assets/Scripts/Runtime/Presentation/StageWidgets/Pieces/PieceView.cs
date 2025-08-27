using UnityEngine;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public sealed class PieceView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
