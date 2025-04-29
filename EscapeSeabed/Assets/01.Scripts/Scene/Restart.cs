using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneChange.instance.LoadStartScene();
        }
    }
}
