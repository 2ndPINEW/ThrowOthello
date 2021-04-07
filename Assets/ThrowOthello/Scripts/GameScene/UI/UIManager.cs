using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    ScoreBoard scoreBoard;

    [SerializeField]
    Text turnNumberText;

    [SerializeField]
    Animator turnAnimator;

    // isSetTurnTriangleがtrueならスコアの高い方にやじるしが傾く
    public void UpdateScoreBoard(int whiteScore, int blackScore, bool isSetTurnTriangle)
    {
        scoreBoard.UpdateScoreText(blackScore, whiteScore);

        if (!isSetTurnTriangle) return;

        if (blackScore > whiteScore) SetTurn(Color.black);
        else if (blackScore < whiteScore) SetTurn(Color.white);
        else SetTurn(Color.gray);
    }

    public void SetTurn(Color color)
    {
        if (color == Color.black) turnAnimator.SetBool("isBlack", true);
        if (color == Color.white) turnAnimator.SetBool("isBlack", false);
    }


    public void updateTurnNumber(int number)
    {
        turnNumberText.text = string.Format("{0:D2}", number);
    }
}
