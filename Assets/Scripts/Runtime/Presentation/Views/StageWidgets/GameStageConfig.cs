using Alchemy.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGSP.TrackPiece.Presentation.Views.StageWidgets
{
    [Serializable]
    public struct GridInfo
    {
        public int xSize;
        public int ySize;
        public float xSpacing;
        public float ySpacing;
    }

    [Serializable]
    public struct StageInfo
    {
        public float cameraDistance;
        public GridInfo gridInfo;
    }

    [CreateAssetMenu(fileName = "GameStageConfig", menuName = "TrackPiece/GameStageConfig")]
    [AlchemySerialize]
    public partial class GameStageConfig : ScriptableObject
    {
        [AlchemySerializeField, NonSerialized]
        public Dictionary<int, StageInfo> stageInfoTable = new();
    }
}
