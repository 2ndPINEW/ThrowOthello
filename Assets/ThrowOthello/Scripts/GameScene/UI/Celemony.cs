using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celemony : MonoBehaviour
{
    [SerializeField]
    GameObject[] targets;
    [SerializeField]
    GameObject[] targets2;
    [SerializeField]
    GameObject targets3;

    [SerializeField]
    SoundManager soundManager;


    public void triger()
    {
        StartCoroutine(Pafu());
        StartCoroutine(loop());
        StartCoroutine(loop2());
        StartCoroutine(loop3());
    }
    IEnumerator Pafu()
    {
        soundManager.PlaySound(SoundManager.SoundType.RESULT_PUF);
        yield return new WaitForSeconds(0.2f);
        soundManager.PlaySound(SoundManager.SoundType.RESULT_PUF);
        yield return new WaitForSeconds(0.2f);
        soundManager.PlaySound(SoundManager.SoundType.RESULT_PUF);
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator loop()
    {
        for(int i = 0;i<targets.Length; i++)
        {
            targets[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator loop2()
    {
        for (int i = 0; i < targets2.Length; i++)
        {
            targets2[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator loop3()
    {
        yield return new WaitForSeconds(3);
        targets3.SetActive(true);
    }
}
