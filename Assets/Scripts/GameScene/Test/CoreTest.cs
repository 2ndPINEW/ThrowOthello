using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello.Core;
using ThrowOthello.Core.Settings;
using System;

public class CoreTest : MonoBehaviour
{
    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    Action[] testFunc;
    Square[] ansSquares;


    private void Start()
    {
        TestSet();
        StartCoroutine(test());
    }


    IEnumerator test()
    {
        for(int i = 0; i < testFunc.Length; i++)
        {
            testFunc[i]();

            while (true)
            {
                if (core.isAllPieceRedy()) break;
                yield return new WaitForSeconds(1);
            }

            core.fieldInitialize();

            core.FieldOrgnize();
            yield return new WaitForSeconds(2);

            core.OthelloCHK();

            yield return new WaitForSeconds(1.5f);

            ui.UpdateScoreBoard(core.CountScore(Color.white), core.CountScore(Color.black));
            core.fieldInitialize();

            if (CompareFieldSquares(core.GetFieldData().Squares, ansSquares))
            {
                Debug.Log(string.Format("テスト{0} OK", i));
            }
            else
            {
                Debug.LogError(string.Format("テスト{0} Failed", i));
            }

            core.ResetGame();
            yield return null;
        }
    }


    bool CompareFieldSquares(Square[] squares0, Square[] squares1)
    {
        if (squares0.Length != squares1.Length) return false;

        for (int i = 0; i < FieldSetting.NumberOfSquares * FieldSetting.NumberOfSquares; i++)
        {
            if (squares0[i].color != squares1[i].color) return false;
        }
        return true;
    }


    Square[] Factory()
    {
        Square[] ansSquares = new Square[0];
        for (int i = 0; i < FieldSetting.NumberOfSquares * FieldSetting.NumberOfSquares; i++)
        {
            Array.Resize(ref ansSquares, ansSquares.Length + 1);
            ansSquares[i] = new Square();
        }
        return ansSquares;
    }


    public void TestSet()
    {
        testFunc = new Action[5];
        testFunc[0] = Test0;
        testFunc[1] = Test1;
        testFunc[2] = Test2;
        testFunc[3] = Test3;
        testFunc[4] = Test4;
    }


    void Test0()
    {
        core.GpecificationGeneratePiece(new PositionIndex(3, 6).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 5).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 4).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(1), Color.white, true);

        ansSquares =  Factory();

        ansSquares[new PositionIndex(3, 6).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 5).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 4).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(3, 3).ToIndex()].color = Color.white;
    }

    void Test1()
    {
        core.GpecificationGeneratePiece(new PositionIndex(3, 6).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 5).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 4).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(1), Color.white, true);

        ansSquares = Factory();

        ansSquares[new PositionIndex(3, 6).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 5).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(3, 4).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 3).ToIndex()].color = Color.white;
    }

    void Test2()
    {
        core.GpecificationGeneratePiece(new PositionIndex(3, 6).ToVector3(0.5f), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 6).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 5).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 4).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(1), Color.white, true);

        ansSquares = Factory();

        ansSquares[new PositionIndex(3, 6).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 5).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 4).ToIndex()].color = Color.white;
        ansSquares[new PositionIndex(3, 3).ToIndex()].color = Color.white;
    }

    void Test3()
    {
        core.GpecificationGeneratePiece(new PositionIndex(0, 0).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(1, 1).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(2, 2).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(4, 4).ToVector3(0.5f), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(4, 4).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(5, 5).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(6, 6).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(7, 7).ToVector3(1), Color.black, true);

        ansSquares = Factory();

        ansSquares[new PositionIndex(0, 0).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(1, 1).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(2, 2).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(3, 3).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(4, 4).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(5, 5).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(6, 6).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(7, 7).ToIndex()].color = Color.black;
    }


    void Test4()
    {
        core.GpecificationGeneratePiece(new PositionIndex(7, 7).ToVector3(0.5f), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(7, 7).ToVector3(1), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(6, 6).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(5, 5).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(4, 4).ToVector3(0.5f), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(4, 4).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(2, 2).ToVector3(1), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(1, 1).ToVector3(0.5f), Color.black, true);
        core.GpecificationGeneratePiece(new PositionIndex(1, 1).ToVector3(1), Color.gray, true);
        core.GpecificationGeneratePiece(new PositionIndex(0, 0).ToVector3(0.5f), Color.white, true);
        core.GpecificationGeneratePiece(new PositionIndex(0, 0).ToVector3(1), Color.black, true);

        ansSquares = Factory();

        ansSquares[new PositionIndex(0, 0).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(1, 1).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(2, 2).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(3, 3).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(4, 4).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(5, 5).ToIndex()].color = Color.black;
        ansSquares[new PositionIndex(6, 6).ToIndex()].color = Color.gray;
        ansSquares[new PositionIndex(7, 7).ToIndex()].color = Color.black;
    }
}
