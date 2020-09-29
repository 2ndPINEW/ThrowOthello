﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello;

public class CoreSample : MonoBehaviour
{
    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    private void Start()
    {
        StartCoroutine(test());
    }


    IEnumerator test()
    {
        while (true)
        {
            core.GenerateRandomPiece(1);

            /*core.GpecificationGeneratePiece(new PositionIndex(3, 6).ToVector3(10), Color.white, true);
            yield return new WaitForSeconds(0.2f);
            core.GpecificationGeneratePiece(new PositionIndex(3, 5).ToVector3(10), Color.black, true);
            yield return new WaitForSeconds(0.2f);
            core.GpecificationGeneratePiece(new PositionIndex(3, 4).ToVector3(10), Color.gray, true);
            yield return new WaitForSeconds(2);
            core.GpecificationGeneratePiece(new PositionIndex(3, 3).ToVector3(10), Color.white, true);*/

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

            if (core.NumberOfPieces() >= 64)
            {
                Debug.Log("終了");
                yield break;
                //core.ResetGame();
            }
        }
    }
}
