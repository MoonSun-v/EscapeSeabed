using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Manager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(NextScene());
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.5f);

        SceneChange.instance.LoadNextScene();
    }
}
