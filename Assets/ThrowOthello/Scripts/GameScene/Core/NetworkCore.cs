using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello.Core;

namespace ThrowOthello.Core.Network
{
    public class NetworkCore
    {
        public string getAllData(ThrowOthelloCore core, MuchInfo muchInfo)
        {
            var AllData = new AllData();
            AllData.PiecePositionAndRotationListObject = core.diffPiecePositionAndRotation();
            AllData.GenerateRequestPieceDatas = null;
            AllData.muchInfo = muchInfo;
            return JsonUtility.ToJson(AllData);
        }

        public AllData JsonToAllData(string json)
        {
            AllData o = JsonUtility.FromJson<AllData>(json);
            return o;
        }
    }

    [System.Serializable]
    public class MuchInfo
    {
        public string muchId;
        public int whiteRemainingNumberOfPieces;
        public int blackRemainingNumberOfPieces;
        public int blackScore;
        public int whiteScore;
        public Color turnColor;
    }

    [System.Serializable]
    public class GenerateRequestPieceData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string playerId;
    }

    [System.Serializable]
    public class PiecePositionAndRotation
    {
        public MoveData moveData;
        public string id;
        public bool isKinematic;
    }

    [System.Serializable]
    public class PiecePositionAndRotationListObject
    {
        public List<PiecePositionAndRotation> PiecePositionAndRotations;
        public int totalPieces;

        public PiecePositionAndRotationListObject()
        {
            PiecePositionAndRotations = new List<PiecePositionAndRotation>();
        }
    }


    public class AllData
    {
        public PiecePositionAndRotationListObject PiecePositionAndRotationListObject;
        public List<GenerateRequestPieceData> GenerateRequestPieceDatas;
        public MuchInfo muchInfo;

        public AllData()
        {
            GenerateRequestPieceDatas = new List<GenerateRequestPieceData>();
        }
    }
}