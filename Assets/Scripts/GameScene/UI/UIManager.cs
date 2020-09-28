using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    ScoreBoard scoreBoard;

    public void UpdateScoreBoard(int whiteScore, int blackScore)
    {
        scoreBoard.UpdateScoreText(blackScore, whiteScore);
    }
}
