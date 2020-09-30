//参考
//https://qiita.com/qavion/items/5d2b74ae51517d9e8d9f
//

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ThrowOthello;

public class AccelerationScript : MonoBehaviour
{
    [SerializeField]
    SPLocalSample spLocalSample;

    private Queue<float> lastXAccels = new Queue<float>();
    private Queue<float> lastYAccels = new Queue<float>();
    private Queue<float> lastZAccels = new Queue<float>();

    // 保持するフレーム数
    private readonly int ACCEL_NUM = 15;

    float xA, yA, zA;

    Vector3 vel;
    float power;

    private void Start()
    {
        for (int i = 0; i < ACCEL_NUM; i++)
        {
            lastXAccels.Enqueue(0);
            lastYAccels.Enqueue(0);
            lastZAccels.Enqueue(0);
        }
    }

    private void Update()
    {
        xA = Input.acceleration.x;
        yA = Input.acceleration.y;
        zA = Input.acceleration.z;

        lastXAccels.Dequeue();
        lastXAccels.Enqueue(xA);
        lastYAccels.Dequeue();
        lastYAccels.Enqueue(yA);
        lastZAccels.Dequeue();
        lastZAccels.Enqueue(zA);

        var vel = CalcVelocity();

        if (Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                power = calcPower();
                spLocalSample.GeneratePiece(new MoveData(transform.forward * power, Input.gyro.rotationRate, transform.position, transform.rotation));
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            spLocalSample.GeneratePiece(new MoveData(transform.forward * 6, Input.gyro.rotationRate, transform.position, transform.rotation));
        }
    }

    float calcPower()
    {
        //var max = 40 - Mathf.Abs(transform.forward.y * 20);
        //var k = 20/max;
        return Mathf.Abs(vel.x);
    }

    public Vector3 CalcVelocity()
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
        vel = vec;
        return vec;
    }

    private void OnGUI()
    {
        var rect = new Rect(30, 30, 800, 50);
        GUI.skin.label.fontSize = 30;
        GUI.Label(rect, string.Format("X={0:F2}, Y={1:F2}, Z={2:F2}, {3}, {4}",
            vel.x, vel.y, vel.z, power, calcPower()));
    }
}