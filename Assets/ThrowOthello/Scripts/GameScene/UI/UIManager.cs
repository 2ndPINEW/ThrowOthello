using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    ScoreBoard scoreBoard;

    [SerializeField]
    Text turnNumberText;

    [SerializeField]
    Animator turnAnimator;

    [SerializeField]
    GameObject ThrowButton;

    [SerializeField]
    GameObject WinImage;

    [SerializeField]
    GameObject LoseImage;

    [SerializeField]
    GameObject DrawImage;

    [SerializeField]
    Celemony celemony;

    [SerializeField]
    GameObject EnemyNameArea;
    [SerializeField]
    Text EnemyNameText;

    [SerializeField]
    GameObject MuchingWaiting;

    [SerializeField]
    GameObject SceneMoveCanvas;
    [SerializeField]
    Animator SceneMoveAnimator;

    private void Start()
    {
        SceneChangeClose();
    }

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

    public void isShowThrowButton(bool value)
    {
        ThrowButton.SetActive(value);
    }

    public void Win()
    {
        isShowThrowButton(false);
        WinImage.SetActive(true);
        celemony.triger();
    }

    public void Lose()
    {
        soundManager.PlaySound(SoundManager.SoundType.LOSE);
        isShowThrowButton(false);
        LoseImage.SetActive(true);
    }

    public void Draw()
    {
        soundManager.PlaySound(SoundManager.SoundType.LOSE);
        isShowThrowButton(false);
        DrawImage.SetActive(true);
    }

    public void ShowEnemyName(string name)
    {
        soundManager.PlaySound(SoundManager.SoundType.MUCHED);
        EnemyNameArea.SetActive(true);
        EnemyNameText.text = "VS: " + name;
        StartCoroutine(closeName());
    }

    IEnumerator closeName()
    {
        yield return new WaitForSeconds(5);
        EnemyNameArea.SetActive(false);
        isShowThrowButton(true);
        soundManager.PlaySound(SoundManager.SoundType.GAME_START);
    }

    public void CloseMuchWaitingDialog()
    {
        MuchingWaiting.SetActive(false);
    }

    public void SceneChangeOpen()
    {
        SceneMoveCanvas.SetActive(true);
        SceneMoveAnimator.SetTrigger("Open");
    }

    public void SceneChangeClose()
    {
        SceneMoveAnimator.SetTrigger("Close");
        StartCoroutine(SceneChangeCloseWait());
    }

    IEnumerator SceneChangeCloseWait()
    {
        yield return new WaitForSeconds(0.4f);
        SceneMoveCanvas.SetActive(false);
    }
}
