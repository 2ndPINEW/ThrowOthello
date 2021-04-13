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
    int TermsEndTurn;
    // 一定以上のスコア差
    int TermsEndScoreDifference;
    // 一ターンあたりの制限時間
    // 時間制
    int TermsEndTimeLimit;

    string playerName = "";


    private void Awake()
    {
        isHost = PlayerPrefs.GetInt("isHost") == 1 ? true : false;
        TermsEndTurn = PlayerPrefs.GetInt("TermsEndTurn");
        TermsEndScoreDifference = PlayerPrefs.GetInt("TermsEndScoreDifference");
        playerName = PlayerPrefs.GetString("PlayerName");
        Debug.Log(playerName);
        uDPServer.isHost = isHost;
        uDPClient.isHost = isHost;
        uDPClient.playerName = playerName;
        uDPClient.TermsEndTurn = TermsEndTurn;
        uDPClient.TermsEndScoreDifference = TermsEndScoreDifference;
        pieceGenerater.isHost = isHost;
        if (isHost) Camera.main.transform.position = new Vector3(0f, 6f, 9f);
        else Camera.main.transform.position = new Vector3(0f, 6f, -9f);
    }
}
