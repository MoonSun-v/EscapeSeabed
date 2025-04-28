using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;   // 싱글톤

    private void Awake()
    {
        // 싱글톤으로 관리
        #region 싱글톤
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
    }

    // 현재 씬 이름을 확인하는 함수
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;  // UnityEngine.SceneManagement의 SceneManager를 사용
    }

    // 다음 씬으로 전환하는 함수 (씬 이름으로 전환)
    public void LoadNextScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "00.Start":
                SceneManager.LoadScene("01.Enter");
                break;
            case "01.Enter":
                SceneManager.LoadScene("02.Stage1");
                break;
        }

    }
}
