using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    [SerializeField]
    UDPClient uDPClient;
    [SerializeField]
    UDPServer uDPServer;
    [SerializeField]
    PieceGenerater pieceGenerater;

    [SerializeField]
    bool isHost;

    // げーむの終了条件
    // ターン制
    // 一定以上のスコア差
    // 一ターンあたりの制限時間
    // 時間制

    private void Awake()
    {
        uDPServer.isHost = isHost;
        uDPClient.isHost = isHost;
        pieceGenerater.isHost = isHost;
        if (isHost) Camera.main.transform.position = new Vector3(0f, 6f, 9f);
        else Camera.main.transform.position = new Vector3(0f, 6f, -9f);
    }
}
