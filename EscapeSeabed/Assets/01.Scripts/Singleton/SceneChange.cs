using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;   // �̱���

    private void Awake()
    {
        // �̱������� ����
        #region �̱���
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

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;  
    }

    // ���� ������ ��ȯ
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
            case "02.Stage1":
                SceneManager.LoadScene("03.Stage2");
                break;
        }

    }
}
