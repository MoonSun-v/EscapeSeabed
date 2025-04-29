using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2.5f);

        SceneChange.instance.LoadNextScene();
    }
}
