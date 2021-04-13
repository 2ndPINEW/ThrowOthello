using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;
using System;
using ThrowOthello.Core;
using ThrowOthello.Core.Settings;
using ThrowOthello.Core.Network;

public class UDPServer : MonoBehaviour
{
    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;
    [SerializeField]
    UDPClient uDPClient;
    [SerializeField]
    SoundManager soundManager;

    public bool isHost;

    bool isGameEnd = false;

    public int LOCA_LPORT;
    private UdpClient udp;
    Thread thread;


    bool pieceGenerated = false;
    bool forceOrganized = false;

    Color turnColor = Color.black;

    string[] ReceiveData = new string[0];
    NetworkCore networkCore = new NetworkCore();

    private void Start()
    {
        if (isHost) LOCA_LPORT = 5234;
        else        LOCA_LPORT = 5123;

        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }

    string name = "";
    string targetip = "";
    bool connect = false;

    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            string text = Encoding.UTF8.GetString(data);
            if(text == "TH+CONNECTED")
            {
                uDPClient.connected = true;
            }
            else if (text.StartsWith("TH+IP_FOUND_"))
            {
                targetip = remoteEP.Address.ToString();
                name = text.Replace("TH+IP_FOUND_", "");
                Debug.Log(text);
            }
            else
            {
                Array.Resize(ref ReceiveData, ReceiveData.Length + 1);
                ReceiveData[ReceiveData.Length - 1] = text;
            }
        }
    }

    private void Update()
    {
        if(targetip != "" && !connect)
        {
            connect = true;
            uDPClient.connect(targetip, name);
        }
        if (ReceiveData.Length > 0)
        {
            for (int i = 0; i < ReceiveData.Length; i++)
            {
                var tmp = ReceiveData[i];
                if (isHost)
                {
                    Debug.Log(tmp);
                    if (tmp == "TH+RESYNC")
                    {
                        uDPClient.NeedReSync();
                    }
                    else
                    {
                        GenerateRequestedPiece(tmp);
                    }
                }
                else
                {
                    if (tmp == "TH+GENERATED")
                    {
                        soundManager.PlaySound(SoundManager.SoundType.THROW);
                    }
                    else
                    {
                        SyncAllData(tmp);
                    }
                }

            }
            ReceiveData = new string[0];
        }
    }

    void GenerateRequestedPiece(string data)
    {
        var tmp = networkCore.JsonToRequestPieceData(data);
        uDPClient.GeneratePiece(tmp.moveData, Color.white);
    }

    void SyncAllData(string data)
    {
        if (isGameEnd) return;
        var tmp = networkCore.JsonToAllData(data);
        if (core.OverWriteAllPiecePositionAndRotation(tmp.PiecePositionAndRotationListObject))
        {
            Debug.Log("再同期のリクエスト送信");
            uDPClient.SendRequestReSync();
        }
        ui.SetTurn(tmp.muchInfo.turnColor);
        ui.UpdateScoreBoard(tmp.muchInfo.whiteScore, tmp.muchInfo.blackScore, false);
        if (tmp.muchInfo.isGameEnd)
        {
            isGameEnd = true;
            if (tmp.muchInfo.whiteScore > tmp.muchInfo.blackScore) ui.Win();
            if (tmp.muchInfo.whiteScore < tmp.muchInfo.blackScore) ui.Lose();
            if (tmp.muchInfo.whiteScore == tmp.muchInfo.blackScore) ui.Draw();
        }
    }

    MoveData JsonToMoveData(string moveData)
    {
        return JsonUtility.FromJson<MoveData>(moveData);
    }
}
