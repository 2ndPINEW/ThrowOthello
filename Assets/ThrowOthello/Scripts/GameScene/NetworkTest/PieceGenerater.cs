using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello.Core;
using System.Linq;

public class PieceGenerater : MonoBehaviour
{
    [SerializeField]
    UDPClient uDPClient;

    public bool isHost;

    private Queue<float> aveXAccels = new Queue<float>();
    private Queue<float> aveYAccels = new Queue<float>();
    private Queue<float> aveZAccels = new Queue<float>();

    private Queue<float> lastXAccels = new Queue<float>();
    private Queue<float> lastYAccels = new Queue<float>();
    private Queue<float> lastZAccels = new Queue<float>();

    // 保持するフレーム数
    private readonly int ACCEL_NUM = 15;

    float xA, yA, zA;

    Vector3 aveVel;
    Vector3 vel;
    float power;

    float tmp_startat = 0;

    private void Start()
    {
        for (int i = 0; i < ACCEL_NUM; i++)
        {
            lastXAccels.Enqueue(0);
            lastYAccels.Enqueue(0);
            lastZAccels.Enqueue(0);
        }

        for (int i = 0; i < ACCEL_NUM * 2; i++)
        {
            aveXAccels.Enqueue(0);
            aveYAccels.Enqueue(0);
            aveZAccels.Enqueue(0);
        }
    }

    public void onGenerateButtonClick()
    {
        StartCoroutine(PieceGenerate());
    }

    private void Update()
    {
        xA = Input.acceleration.x;
        yA = Input.acceleration.y;
        zA = Input.acceleration.z;

        aveXAccels.Dequeue();
        aveXAccels.Enqueue(lastXAccels.Peek());
        aveYAccels.Dequeue();
        aveYAccels.Enqueue(lastYAccels.Peek());
        aveZAccels.Dequeue();
        aveZAccels.Enqueue(lastZAccels.Peek());

        lastXAccels.Dequeue();
        lastXAccels.Enqueue(xA);
        lastYAccels.Dequeue();
        lastYAccels.Enqueue(yA);
        lastZAccels.Dequeue();
        lastZAccels.Enqueue(zA);

        aveVel = CalcVelocity(aveXAccels, aveYAccels, aveZAccels);
        vel = CalcVelocity(lastXAccels, lastYAccels, lastZAccels);

        /*if (Input.GetKeyDown(KeyCode.A))
        {
            if (isHost) uDPClient.GeneratePiece(new MoveData(transform.forward * 6, Input.gyro.rotationRate, transform.position, transform.rotation), Color.black);
            else uDPClient.GeneratePiece(new MoveData(transform.forward * 6, Input.gyro.rotationRate, transform.position, transform.rotation), Color.white);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 velocity = new Vector3(Random.Range(5f, 15f), Random.Range(-5f, 0f), Random.Range(-5f, 5f));
            Vector3 angularVelocity = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(-20f, 20f));
            Quaternion quaternion = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            MoveData moveData = new MoveData(velocity, angularVelocity, Camera.main.transform.position, quaternion);
            if (isHost) uDPClient.GeneratePiece(moveData, Color.black);
            else uDPClient.GeneratePiece(moveData, Color.white);
        }*/
    }


    IEnumerator PieceGenerate()
    {
        yield return new WaitForSeconds(0.15f);
        Vector3 power = calcPower();
        var lotation = new Vector3(Input.gyro.rotationRate.z, Input.gyro.rotationRate.y, Input.gyro.rotationRate.x);
        MoveData moveData = new MoveData(transform.forward * power.x + transform.up * power.y, lotation, transform.position, transform.rotation);
        if (isHost) moveData.Rotation = Quaternion.Euler(90, 0, 0);
        else moveData.Rotation = Quaternion.Euler(270, 0, 0);
        if (isHost) uDPClient.GeneratePiece(moveData, Color.black);
        else uDPClient.GeneratePiece(moveData, Color.white);
    }

    Vector3 calcPower()
    {
        return new Vector3(Mathf.Abs(aveVel.x - vel.x), Mathf.Abs(aveVel.y - vel.y), Mathf.Abs(aveVel.z - vel.y));
    }

    public Vector3 CalcVelocity(Queue<float> lastXAccels, Queue<float> lastYAccels, Queue<float> lastZAccels)
    {
        float xVelocity;
        float yVelocity;
        float zVelocity;

        // |z|の最大値
        var zMax = lastZAccels.Select(z => Mathf.Abs(z)).Max();
        // x方向の速度（奥行方向の速度をzMaxを元に計算）
        xVelocity = Mathf.Clamp(zMax, 0, 2) * 20;

        // yの絶対値平均誤差
        var yAverage = lastYAccels.Average();
        var yVariance = lastYAccels
            .Select(y => Mathf.Abs(y - yAverage))
            .Sum() / lastYAccels.Count;
        yVelocity = yVariance * Mathf.Sign(lastYAccels.Last()) * 15;

        // xの絶対値平均誤差
        var xAverage = lastXAccels.Average();
        var xVariance = lastXAccels
            .Select(x => Mathf.Abs(x - xAverage))
            .Sum() / lastZAccels.Count;
        zVelocity = xVariance * Mathf.Sign(lastXAccels.Last()) * 20;

        var vec = new Vector3(xVelocity, yVelocity, zVelocity);
        return vec;
    }
}
