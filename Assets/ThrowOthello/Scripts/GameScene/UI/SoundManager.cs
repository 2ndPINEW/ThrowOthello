using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    AudioSource[] audioSources;

    public AudioClip pop_motion;
    public AudioClip game_start;
    public AudioClip result;
    public AudioClip piece_throw;
    public AudioClip muched;
    public AudioClip lose;


    public enum SoundType
    {
        POP_MOTION,     //ぴょこって音
        GAME_START,     //ゲーム開始
        RESULT_PUF,     //ぱふぱふ
        THROW,          //投げる
        MUCHED,         //マッチ完了
        LOSE,
    }

    private void Start()
    {
        audioSources = gameObject.GetComponents<AudioSource>();
    }

    AudioClip audioClip(SoundType type)
    {
        switch (type)
        {
            case SoundType.POP_MOTION:
                return pop_motion;
            case SoundType.GAME_START:
                return game_start;
            case SoundType.RESULT_PUF:
                return result;
            case SoundType.THROW:
                return piece_throw;
            case SoundType.MUCHED:
                return muched;
            case SoundType.LOSE:
                return lose;
            default:
                return null;
        }
    }

    public void PlaySound(SoundType type)
    {
        var audio = audioClip(type);
        audioSources[0].PlayOneShot(audio);
    }
}
