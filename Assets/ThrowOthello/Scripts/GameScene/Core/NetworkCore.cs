using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello.Core;

namespace ThrowOthello.Core.Network
{
    public class NetworkCore
    {
        public string getAllData(ThrowOthelloCore core, MuchInfo muchInfo, bool isNeedReSync)
        {
            var AllData = new AllData();
            if(isNeedReSync)
                AllData.PiecePositionAndRotationListObject = core.allPiecePositionAndRotationObject();
            else
                AllData.PiecePositionAndRotationListObject = core.diffPiecePositionAndRotation();
            AllData.muchInfo = muchInfo;
            return JsonUtility.ToJson(AllData);
        }

        public AllData JsonToAllData(string json)
        {
            AllData o = JsonUtility.FromJson<AllData>(json);
            return o;
        }

        public string GenerateRequestPieceDataToJson(GenerateRequestPieceData generateRequestPieceData)
        {
            return JsonUtility.ToJson(generateRequestPieceData);
        }

        public GenerateRequestPieceData JsonToRequestPieceData(string json)
        {
            GenerateRequestPieceData o = JsonUtility.FromJson<GenerateRequestPieceData>(json);
            return o;
        }
    }

    [System.Serializable]
    public class PlayerObject
    {
        public Vector3 position;
        public Color team;
        public string pid;
    }

    [System.Serializable]
    public class CoreData
    {
        public bool isFieldOrganizing;
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
        public bool isGameEnd = false;
        public bool canGeneratePiece = false;
    }

    [System.Serializable]
    public class GenerateRequestPieceData
    {
        public MoveData moveData;
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
        public bool hasAllPiece;

        public PiecePositionAndRotationListObject()
        {
            PiecePositionAndRotations = new List<PiecePositionAndRotation>();
        }
    }


    public class AllData
    {
        public PiecePositionAndRotationListObject PiecePositionAndRotationListObject;
        public MuchInfo muchInfo;
        public List<PlayerObject> playerObjects;
    }
}