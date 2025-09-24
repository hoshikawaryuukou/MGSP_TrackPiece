using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace MGSP.TrackPiece.Presentation.Views.StageWidgets
{
    public sealed class GridView : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private Vector3[] gridPositions;

        [Button]
        public void Arrange(int xSize, int ySize, float xSpacing, float ySpacing)
        {
            gridPositions = new Vector3[xSize * ySize];

            var index = 0;
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    float posX = (x - (xSize - 1) * 0.5f) * xSpacing;
                    float posY = ((ySize - 1) * 0.5f - y) * ySpacing;
                    gridPositions[index] = new Vector3(posX, posY, 0);
                    index++;
                }
            }
        }

        public Vector3 GetPosition(int index)
        {
            return gridPositions[index];
        }
    }
}
