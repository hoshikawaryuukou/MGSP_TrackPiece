#if UNITY_EDITOR

using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.StageWidgets
{
    public partial class GameStageView
    {
        [BoxGroup("Editor_Tools"), SerializeField] private GameObject _tempObj;
        [BoxGroup("Editor_Tools"), SerializeField] private Transform _tempObjRoot;
        [BoxGroup("Editor_Tools"), SerializeField] private List<Transform> _tempObjs = new();
        [BoxGroup("Editor_Tools"), OnValueChanged("_ApplyPositions"), SerializeField] private float _colSpacing = 1.1f;
        [BoxGroup("Editor_Tools"), OnValueChanged("_ApplyPositions"), SerializeField] private float _rowSpacing = 1.1f;
        [BoxGroup("Editor_Tools"), OnValueChanged("_ApplyCameraPosition"), SerializeField] private float _cameraDistance = 5.0f;
        [BoxGroup("Editor_Tools"), Button] private void _Load_4x4_Config() => _Load_Config(4);
        [BoxGroup("Editor_Tools"), Button] private void _Save_4x4_Config() => _Save_Config(4);
        [BoxGroup("Editor_Tools"), Button] private void _Load_6x6_Config() => _Load_Config(6);
        [BoxGroup("Editor_Tools"), Button] private void _Save_6x6_Config() => _Save_Config(6);

        private int _cols = 4;
        private int _rows = 4;

        private void _Load_Config(int level)
        {
            var stageInfo = gameStageConfig.stageInfoTable[level];
            _cameraDistance = stageInfo.cameraDistance;

            var gridInfo = stageInfo.gridInfo;
            _cols = gridInfo.xSize;
            _rows = gridInfo.ySize;
            _colSpacing = gridInfo.xSpacing;
            _rowSpacing = gridInfo.ySpacing;

            _ClearTempObjs();
            _FillTempObjs();
            _ApplyCameraPosition();
        }

        private void _Save_Config(int level)
        {
            var stageInfo = gameStageConfig.stageInfoTable[level];
            stageInfo.cameraDistance = _cameraDistance;

            var gridInfo = stageInfo.gridInfo;
            gridInfo.xSize = _cols;
            gridInfo.ySize = _rows;
            gridInfo.xSpacing = _colSpacing;
            gridInfo.ySpacing = _rowSpacing;
            stageInfo.gridInfo = gridInfo;

            gameStageConfig.stageInfoTable[level] = stageInfo;

            UnityEditor.EditorUtility.SetDirty(gameStageConfig);
        }

        [BoxGroup("Editor_Tools"), Button]
        private void _ClearTempObjs()
        {
            if (_tempObjRoot == null) return;

            for (int i = _tempObjRoot.childCount - 1; i >= 0; i--)
            {
                var child = _tempObjRoot.GetChild(i);
                if (child != null)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
            _tempObjs.Clear();
        }

        private void _FillTempObjs()
        {
            if (_tempObj == null || _tempObjRoot == null)
                return;

            _ClearTempObjs();

            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var symbolViewObj = UnityEditor.PrefabUtility.InstantiatePrefab(_tempObj) as GameObject;
                    var symbolViewTransform = symbolViewObj.transform;
                    symbolViewTransform.SetParent(_tempObjRoot, false);
                    _tempObjs.Add(symbolViewTransform);
                }
            }

            _ApplyPositions();
            _ApplyCameraPosition();
        }

        private void _ApplyPositions()
        {
            if (_tempObjRoot == null || _tempObjs.Count == 0) return;

            if (gridView != null)
            {
                gridView.Arrange(_cols, _rows, _colSpacing, _rowSpacing);
                for (int i = 0; i < _cols; i++)
                {
                    for (int j = 0; j < _rows; j++)
                    {
                        var index = i * _rows + j;
                        if (index < _tempObjs.Count)
                        {
                            _tempObjs[index].position = gridView.GetPosition(index);
                        }
                    }
                }
            }
        }

        private void _ApplyCameraPosition()
        {
            if (stageCamera == null || _tempObjRoot == null)
                return;

            stageCamera.orthographicSize = _cameraDistance;
        }

    }
}
#endif