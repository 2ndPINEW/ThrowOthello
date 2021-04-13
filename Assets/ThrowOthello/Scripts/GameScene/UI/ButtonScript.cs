using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    GameObject RoomSettingCanvas;
    [SerializeField]
    GameObject RoomMakeCanvas;
    [SerializeField]
    InputField playerNameField;
    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    GameObject SceneMoveCanvas;
    [SerializeField]
    Animator SceneMoveAnimator;

    public void OnClicked(Button button)
    {
        soundManager.PlaySound(SoundManager.SoundType.POP_MOTION);
        switch (button.name)
        {
            // ホストとしてげーむの読み込み
            case "GameStartButton":
                PlayerPrefs.SetInt("isHost", 1);
                SceneChangeOpen("NetworkTest");
                break;
            // ゲームの設定開く
            case "RoomSetting":
                if (playerNameField.text == "" || playerNameField.text.Length > 5) return;
                RoomMakeCanvas.SetActive(false);
                RoomSettingCanvas.SetActive(true);
                break;
            // クライアントとしてゲームの読み込み
            case "RoomIn":
                if (playerNameField.text == "" || playerNameField.text.Length > 5) return;
                PlayerPrefs.SetInt("isHost", 0);
                SceneChangeOpen("NetworkTest");
                break;
            case "GoTitleButton(Win)":
                SceneChangeOpen("Title");
                break;
            case "GoTitleButton(Lose)":
                SceneChangeOpen("Title");
                break;
            case "GoTitleButton(Draw)":
                SceneChangeOpen("Title");
                break;
        }
    }

    public void SceneChangeOpen(string moveScene)
    {
        SceneMoveCanvas.SetActive(true);
        SceneMoveAnimator.SetTrigger("Open");
        StartCoroutine(SceneChangeOpenWait(moveScene));
    }

    IEnumerator SceneChangeOpenWait(string moveScene)
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(moveScene);
    }
}
