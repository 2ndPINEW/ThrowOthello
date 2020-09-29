using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello;

public class CoreSample : MonoBehaviour
{
    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    bool pieceGenerated = false;

    Color turnColor = Color.black;

    private void Start()
    {
        StartCoroutine(test());
    }


    public void GeneratePiece(MoveData moveData)
    {
        if (pieceGenerated) return;

        if(turnColor == Color.black) moveData.Rotation = Quaternion.Euler(90, 0, 0);
        if(turnColor == Color.white) moveData.Rotation = Quaternion.Euler(270, 0, 0);
        core.GeneratePiece(moveData, true);
        pieceGenerated = true;
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());
    }


    IEnumerator test()
    {
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());

        while (true)
        {
            ui.SetTurn(turnColor);

            pieceGenerated = false;

            while (true)
            {
                if (core.isAllPieceRedy() && pieceGenerated) break;
                yield return new WaitForSeconds(1);
            }


            core.fieldInitialize();

            core.FieldOrgnize();
            yield return new WaitForSeconds(2);

            core.OthelloCHK();

            yield return new WaitForSeconds(1.5f);

            ui.UpdateScoreBoard(core.CountScore(Color.white), core.CountScore(Color.black));

            if (core.NumberOfPieces() >= FieldSetting.NumberOfPieces)
            {
                Debug.Log("終了");
                yield break;
            }
            if (turnColor == Color.black) turnColor = Color.white;
            else turnColor = Color.black;
        }
    }
}
