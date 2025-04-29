using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static SceneChange instance;   // ΩÃ±€≈Ê

    private void Awake()
    {
        // ΩÃ±€≈Ê¿∏∑Œ ∞¸∏Æ
        #region ΩÃ±€≈Ê
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

    // ¥Ÿ¿Ω æ¿¿∏∑Œ ¿¸»Ø
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
            case "03.Stage2":
                SceneManager.LoadScene("04.Stage3");
                break;
            case "04.Stage3":
                SceneManager.LoadScene("05.Stage4");
                break;
            case "05.Stage4":
                SceneManager.LoadScene("06.Stage5");
                break;
            case "06.Stage5":
                SceneManager.LoadScene("07.Ending");
                break;

        }

    }
}
