using System.Collections;
using System.Collections.Generic;
using ThrowOthello.Core;
using ThrowOthello.Core.Settings;
using UnityEngine;

public class RandomGenerateTestClient : MonoBehaviour
{

    [SerializeField]
    ThrowOthelloCore core;
    [SerializeField]
    UIManager ui;

    bool pieceGenerated = false;

    Color turnColor = Color.black;

    public void GeneratePiece(MoveData moveData)
    {
        if (pieceGenerated) return;

        if (turnColor == Color.black) moveData.Rotation = Quaternion.Euler(90, 0, 0);
        if (turnColor == Color.white) moveData.Rotation = Quaternion.Euler(270, 0, 0);
        core.GeneratePiece(moveData, true);
        pieceGenerated = true;
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
    }


    IEnumerator test()
    {
        ui.updateTurnNumber(FieldSetting.NumberOfPieces - core.NumberOfPieces());

        while (true)
        {
            ui.SetTurn(turnColor);

            pieceGenerated = false;

            Vector3 velocity = new Vector3(Random.Range(5f, 15f), Random.Range(-5f, 0f), Random.Range(-5f, 5f));
            Vector3 angularVelocity = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(-20f, 20f));
            Quaternion quaternion = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            GeneratePiece(new MoveData(velocity, angularVelocity, Camera.main.transform.position, quaternion));

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
                core.ResetGame();
                turnColor = Color.white;
                //yield break;
            }
            if (turnColor == Color.black) turnColor = Color.white;
            else turnColor = Color.black;
        }
    }
}
