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

//クライアント
public class RandomGenerateTestClient : MonoBehaviour
{


    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    public int LOCA_LPORT;
    private UdpClient udp;
    Thread thread;


    bool pieceGenerated = false;
    bool forceOrganized = false;

    Color turnColor = Color.black;

    string[] allDataJsons = new string[0];
    NetworkCore networkCore = new NetworkCore();

    private void Start()
    {
        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
        StartCoroutine(test());
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }


    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            string text = Encoding.UTF8.GetString(data);
            Array.Resize(ref allDataJsons, allDataJsons.Length + 1);
            allDataJsons[allDataJsons.Length - 1] = text;
        }
    }

    private void Update()
    {
        if (allDataJsons.Length > 0)
        {
            for (int i = 0; i < allDataJsons.Length; i++)
            {
                var tmp = networkCore.JsonToAllData(allDataJsons[i]);
                core.OverWriteAllPiecePositionAndRotation(tmp.PiecePositionAndRotationListObject);
                ui.SetTurn(tmp.muchInfo.turnColor);
                ui.UpdateScoreBoard(tmp.muchInfo.whiteScore, tmp.muchInfo.blackScore, false);

            }
            allDataJsons = new string[0];
        }
    }

    MoveData JsonToMoveData(string moveData)
    {
        return JsonUtility.FromJson<MoveData>(moveData);
    }


    IEnumerator test()
    {
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());

        while (true)
        {
            ui.SetTurn(turnColor);

            pieceGenerated = false;
            forceOrganized = false;

            while (true)
            {
                if (pieceGenerated && forceOrganized) break;
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("break");

            yield return new WaitForSeconds(1.6f);

            //ここでコマの全座標調整
            core.fieldInitialize();

            core.FieldOrgnize();
            yield return new WaitForSeconds(2);

            core.OthelloCHK();

            yield return new WaitForSeconds(1.5f);

            ui.UpdateScoreBoard(core.CountScore(Color.white), core.CountScore(Color.black), false);

            if (core.NumberOfPieces() >= FieldSetting.NumberOfPieces)
            {
                Debug.Log("終了");
                core.ResetGame();
                turnColor = Color.white;
                //yield break;
            }
            if (turnColor == Color.black) turnColor = Color.white;
            else turnColor = Color.black;
        }
    }

}
