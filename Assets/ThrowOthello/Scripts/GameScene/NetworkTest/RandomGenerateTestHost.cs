using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ThrowOthello.Core;
using ThrowOthello.Core.Settings;
using UnityEngine;
using ThrowOthello.Core.Network;

//ホスト
public class RandomGenerateTestHost : MonoBehaviour
{

    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    public string host;
    public int port;
    private UdpClient client;

    NetworkCore networkCore = new NetworkCore();
    MuchInfo muchInfo = new MuchInfo();

    bool pieceGenerated = false;


    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);

        muchInfo.turnColor = Color.black;

        StartCoroutine(test());
        StartCoroutine(ClientSync());
    }

    IEnumerator ClientSync()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.04f);
            muchInfo.muchId = "";
            muchInfo.whiteRemainingNumberOfPieces = 0;
            muchInfo.blackRemainingNumberOfPieces = 0;
            SendData(networkCore.getAllData(core, muchInfo));
            //Debug.Log(core.diffPiecePositionAndRotation().Length);
        }
    }


    IEnumerator test()
    {
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());

        while (true)
        {
            ui.SetTurn(muchInfo.turnColor);

            Vector3 velocity = new Vector3(Random.Range(5f, 15f), Random.Range(-5f, 0f), Random.Range(-5f, 5f));
            Vector3 angularVelocity = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(-20f, 20f));
            Quaternion quaternion = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            MoveData moveData = new MoveData(velocity, angularVelocity, Camera.main.transform.position, quaternion);
            core.GeneratePiece(moveData, "", true);

            while (true)
            {
                if (core.isAllPieceRedy()) break;
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

            yield return new WaitForSeconds(5);


            muchInfo.whiteScore = core.CountScore(Color.white);
            muchInfo.blackScore = core.CountScore(Color.black);
            ui.UpdateScoreBoard(muchInfo.whiteScore, muchInfo.blackScore, false);

            if (core.NumberOfPieces() >= FieldSetting.NumberOfPieces)
            {
                Debug.Log("終了");
                core.ResetGame();
                muchInfo.turnColor = Color.white;
                //yield break;
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
}
