using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public sealed class PieceUnit
    {
        public PieceView pieceView;
        public int positionIndex;
    }

    public partial class GameStageView : MonoBehaviour
    {
        [SerializeField] private GameStageConfig gameStageConfig;
        [SerializeField] private Camera stageCamera;
        [SerializeField] private GridView gridView;
        [SerializeField] private CellViewPool cellViewPool;
        [SerializeField] private PieceViewPool pieceViewPool;
        [SerializeField] private PieceViewShiftTweener pieceViewShiftTweener;

        private readonly List<CellView> cellViews = new();
        private readonly List<PieceUnit> pieceUnits = new();

        public void Arrange(int levelSize)
        {
            ClearCellViews();
            ClearPieceUnits();

            var stageInfo = gameStageConfig.stageInfoTable[levelSize];

            stageCamera.orthographicSize = stageInfo.cameraDistance;

            var gridInfo = stageInfo.gridInfo;
            gridView.Arrange(gridInfo.xSize, gridInfo.ySize, gridInfo.xSpacing, gridInfo.ySpacing);

            var cellCount = gridInfo.xSize * gridInfo.ySize;
            for (var i = 0; i < cellCount; i++)
            {
                var cellView = cellViewPool.Get();
                cellView.transform.position = gridView.GetPosition(i);
                cellView.positionIndex = i;
                cellViews.Add(cellView);
            }
        }

        public void Place(int positionIndex, PieceType pieceType)
        {
            var pieceView = pieceViewPool.Get(pieceType);
            pieceView.transform.position = gridView.GetPosition(positionIndex);

            var pieceUnit = new PieceUnit
            {
                pieceView = pieceView,
                positionIndex = positionIndex
            };

            pieceUnits.Add(pieceUnit);
        }

        public async UniTask Shift(IReadOnlyList<int> track)
        {
            var pieceUnitCount = pieceUnits.Count;

            var tasks = new UniTask[pieceUnitCount];

            for (var i = 0; i < pieceUnitCount; i++)
            {
                var pieceUnit = pieceUnits[i];
                var currentPositionIndex = pieceUnit.positionIndex;
                var currentPosition = gridView.GetPosition(currentPositionIndex);
                var targetPositionIndex = track[currentPositionIndex];
                var targetPosition = gridView.GetPosition(targetPositionIndex);

                var task = pieceViewShiftTweener.Run(pieceUnit.pieceView.transform, currentPosition, targetPosition);
                tasks[i] = task;
            }

            await UniTask.WhenAll(tasks);

            for (var i = 0; i < pieceUnitCount; i++)
            {
                var pieceUnit = pieceUnits[i];
                pieceUnit.positionIndex = track[pieceUnit.positionIndex];
            }

            await UniTask.Yield();
        }

        private void ClearCellViews()
        {
            for (var i = 0; i < cellViews.Count; i++)
            {
                var cellView = cellViews[i];
                cellViewPool.Release(cellView);
            }

            cellViews.Clear();
        }

        private void ClearPieceUnits()
        {
            for (var i = 0; i < pieceUnits.Count; i++)
            {
                var pieceUnit = pieceUnits[i];
                pieceViewPool.Release(pieceUnit.pieceView);
            }

            pieceUnits.Clear();
        }


    }
}
