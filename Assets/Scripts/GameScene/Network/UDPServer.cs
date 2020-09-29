using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;
using System;

public class UDPServer : MonoBehaviour
{
    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    public int LOCA_LPORT;
    private UdpClient udp;
    Thread thread;


    string[] moveDataJsons = new string[0];
    string[] transformDataJsons = new string[0];

    bool PieceTransformDataReceived = false;

    private void Start()
    {
        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
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
            Debug.Log(text);
            if (text.StartsWith("PieceTransformdData:"))
            {
                text = text.Replace("PieceTransformdData:", "");
                PieceTransform pieceTransform = JsonUtility.FromJson<PieceTransform>(text);

                Array.Resize(ref transformDataJsons, transformDataJsons.Length + 1);
                transformDataJsons[transformDataJsons.Length - 1] = text;
            }
            if (text.StartsWith("PieceData:"))
            {
                text = text.Replace("PieceData:", "");
                Array.Resize(ref moveDataJsons, moveDataJsons.Length + 1);
                moveDataJsons[moveDataJsons.Length - 1] = text;
            }
        }
    }


    IEnumerator organize()
    {
        while (true)
        {
            if (core.isAllPieceRedy() && PieceTransformDataReceived)
            {
                PieceTransformDataReceived = false;
                break;
            }
            yield return new WaitForSeconds(1);
        }


        core.fieldInitialize();

        core.FieldOrgnize();
        yield return new WaitForSeconds(2);

        core.OthelloCHK();

        yield return new WaitForSeconds(1.5f);

        ui.UpdateScoreBoard(core.CountScore(Color.white), core.CountScore(Color.black));

        if (core.NumberOfPieces() >= 64)
        {
            Debug.Log("終了");
            core.ResetGame();
            yield return new WaitForSeconds(5);
        }

    }


    private void Update()
    {
        if(moveDataJsons.Length > 0)
        {
            for(int i = 0; i < moveDataJsons.Length; i++)
            {
                Generate(JsonToMoveData(moveDataJsons[i]));
            }
            moveDataJsons = new string[0];
            StartCoroutine(organize());
        }

        if (transformDataJsons.Length > 0)
        {
            for(int i = 0; i < transformDataJsons.Length; i++)
            {
                core.OverwitePiceTransform(JsonToTransformData(transformDataJsons[i]));
            }
            transformDataJsons = new string[0];
            PieceTransformDataReceived = true;
        }
    }


    void Generate(MoveData moveData)
    {
        core.GeneratePiece(moveData, true);
    }


    MoveData JsonToMoveData(string moveData)
    {
        return JsonUtility.FromJson<MoveData>(moveData);
    }


    PieceTransform JsonToTransformData(string transformData)
    {
        return JsonUtility.FromJson<PieceTransform>(transformData);
    }
}
