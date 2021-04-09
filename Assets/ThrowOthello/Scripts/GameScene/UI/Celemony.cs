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

    public void triger()
    {
        StartCoroutine(loop());
        StartCoroutine(loop2());
        StartCoroutine(loop3());
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
        for (int i = 0; i < targets.Length; i++)
        {
            targets2[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator loop3()
    {
        yield return new WaitForSeconds(3);
        targets3.SetActive(true);
    }
}
