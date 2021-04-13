using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCounter : MonoBehaviour
{
    [SerializeField]
    Text termsEndTurnNumberText;
    [SerializeField]
    Slider termsEndTurnNumberSlider;

    [SerializeField]
    Text termsEndScoreDifferenceText;
    [SerializeField]
    Slider termsEndScoreDifferenceSlider;

    [SerializeField]
    InputField playerNameField;

    private void Start()
    {
        termsEndTurnNumberSlider.onValueChanged.AddListener((value) =>
        {
            onTermsEndTurnChanged(value);
        });

        termsEndScoreDifferenceSlider.onValueChanged.AddListener((value) =>
        {
            onTermsEndScoreDifferenceChanged(value);
        });

        playerNameField.onValueChange.AddListener((value) =>
        {
            onPlayerNameChanged(value);
        });

        onTermsEndTurnChanged(8f);
        onTermsEndScoreDifferenceChanged(0f);
    }

    void onTermsEndTurnChanged(float value)
    {
        termsEndTurnNumberText.text = (value * 2).ToString();
        PlayerPrefs.SetInt("TermsEndTurn", (int)(value * 2));
    }

    void onTermsEndScoreDifferenceChanged(float value)
    {
        termsEndScoreDifferenceText.text = value.ToString();
        PlayerPrefs.SetInt("TermsEndScoreDifference", (int)(value));
    }

    public void onPlayerNameChanged(string value)
    {
        PlayerPrefs.SetString("PlayerName", value);
    }
}
