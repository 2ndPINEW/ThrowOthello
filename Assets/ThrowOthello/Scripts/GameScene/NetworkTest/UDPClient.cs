using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ThrowOthello.Core;
using ThrowOthello.Core.Settings;
using UnityEngine;
using ThrowOthello.Core.Network;
using System.Net;

public class UDPClient : MonoBehaviour
{

    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;
    [SerializeField]
    UDPServer uDPServer;
    [SerializeField]
    SoundManager soundManager;

    public bool isHost;
    public string playerName;
    public int TermsEndTurn;
    public int TermsEndScoreDifference;

    public int port;
    private UdpClient client;
    private UdpClient bclient;

    NetworkCore networkCore = new NetworkCore();
    MuchInfo muchInfo = new MuchInfo();

    bool pieceGenerated = false;

    bool isNeedReSync = false;

    string ip;
    void Start()
    {
        if (isHost) port = 5123;
        else port = 5234;

        if(isHost) muchInfo.turnColor = Color.black;
        else muchInfo.turnColor = Color.white;

        StartCoroutine(Beacon());
    }

    public bool connected = false;

    IEnumerator Beacon()
    {
        while (true)
        {
            if (connected) break;
            Debug.Log("Beacon");
            bclient = new UdpClient();
            bclient.Connect(IPAddress.Broadcast, port);
            byte[] dgram = Encoding.UTF8.GetBytes("TH+IP_FOUND_" + playerName);
            bclient.Send(dgram, dgram.Length);
            bclient.Close();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void connect(string ip, string name)
    {
        this.ip = ip;
        client = new UdpClient();
        client.Connect(ip, port);
        SendData("TH+CONNECTED");
        ui.CloseMuchWaitingDialog();
        ui.ShowEnemyName(name);

        if (isHost)
        {
            StartCoroutine(HostLogic());
            StartCoroutine(ClientSync());
        }
    }

    public void SendRequestReSync()
    {
        SendData("TH+RESYNC");
    }

    int reSyncTryCount = 0;
    public void NeedReSync()
    {
        Debug.Log("再同期のリクエスト");
        reSyncTryCount = 0;
        isNeedReSync = true;
    }

    //ピースの生成
    public void GeneratePiece(MoveData moveData, Color color)
    {
        if (color != muchInfo.turnColor) return;
        GenerateRequestPieceData generateRequestPieceData = new GenerateRequestPieceData();
        generateRequestPieceData.moveData = moveData;
        generateRequestPieceData.playerId = "";
        if (!isHost) requestGeneratePiece(generateRequestPieceData);
        if (!muchInfo.canGeneratePiece) return;
        soundManager.PlaySound(SoundManager.SoundType.THROW);
        if (isHost) SendData("TH+GENERATED");
        core.GeneratePiece(moveData, "", true);
        muchInfo.canGeneratePiece = false;
    }

    void requestGeneratePiece(GenerateRequestPieceData generateRequestPieceData)
    {
        SendData(networkCore.GenerateRequestPieceDataToJson(generateRequestPieceData));
    }

    IEnumerator ClientSync()
    {
        while (true)
        {
            if (core.isAllPieceRedy()) yield return new WaitForSeconds(0.05f);
            else yield return new WaitForSeconds(0.02f);
            muchInfo.muchId = "";
            muchInfo.whiteRemainingNumberOfPieces = 0;
            muchInfo.blackRemainingNumberOfPieces = 0;
            SendData(networkCore.getAllData(core, muchInfo, isNeedReSync));
            isNeedReSync = false;
        }
    }


    IEnumerator HostLogic()
    {
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());

        while (true)
        {
            ui.SetTurn(muchInfo.turnColor);
            muchInfo.canGeneratePiece = true;

            while (true)
            {
                if (core.isAllPieceRedy() && !muchInfo.canGeneratePiece) break;
                yield return new WaitForSeconds(0.01f);
            }

            core.fieldInitialize();

            core.FieldOrgnize();
            yield return new WaitForSeconds(2);

            if (core.OthelloCHK())
            {
                Debug.Log("オセロ！！！！！！！！！！！！！！");
                yield return new WaitForSeconds(1.5f);
            }

            muchInfo.whiteScore = core.CountScore(Color.white);
            muchInfo.blackScore = core.CountScore(Color.black);
            ui.UpdateScoreBoard(muchInfo.whiteScore, muchInfo.blackScore, false);

            if (core.NumberOfPieces() >= TermsEndTurn || (Mathf.Abs(core.CountScore(Color.black)-core.CountScore(Color.white)) >= TermsEndScoreDifference && TermsEndScoreDifference != 0))//FieldSetting.NumberOfPieces)
            {
                muchInfo.isGameEnd = true;
                if (muchInfo.whiteScore > muchInfo.blackScore) ui.Lose();
                if (muchInfo.whiteScore < muchInfo.blackScore) ui.Win();
                if (muchInfo.whiteScore == muchInfo.blackScore) ui.Draw();
                break;
            }
            if (muchInfo.turnColor == Color.black) muchInfo.turnColor = Color.white;
            else muchInfo.turnColor = Color.black;
        }
    }

    void SendData(string data)
    {
        byte[] dgram = Encoding.UTF8.GetBytes(data);
        client.Send(dgram, dgram.Length);
    }

    void OnApplicationQuit()
    {
        client.Close();
    }

    private void OnGUI()
    {
        var rect = new Rect(30, 30, 800, 50);
        GUI.skin.label.fontSize = 30;
        GUI.Label(rect, ip);
    }
}
