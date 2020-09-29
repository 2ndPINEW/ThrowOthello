﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello;
using System.Net.Sockets;
using System.Text;

public class UDPClient : MonoBehaviour
{

    public string host;
    public int port;
    private UdpClient client;

    private void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);
    }


    string moveDataToJson(MoveData moveData)
    {
        return JsonUtility.ToJson(moveData);
    }


    public void SendPieceData(MoveData moveData)
    {
        Debug.Log(moveDataToJson(moveData));
        SendData(string.Format("PieceData:{0}" ,moveDataToJson(moveData)));
    }


    public void SendFieldData(MoveData moveData)
    {
        Debug.Log(moveDataToJson(moveData));
        SendData(string.Format("FieldData:{0}", moveDataToJson(moveData)));
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
