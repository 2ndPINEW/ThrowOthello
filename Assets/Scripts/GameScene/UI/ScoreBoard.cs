using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    Text WhiteScoreText;

    [SerializeField]
    Text BlackScoreText;

    public void UpdateScoreText(int blackScore, int whiteScore)
    {
        WhiteScoreText.text = string.Format("{0:D2}", whiteScore);
        BlackScoreText.text = string.Format("{0:D2}", blackScore);
    }
}
