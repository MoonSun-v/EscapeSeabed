using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject player;
    public GameObject electric;
    PlayerFSM PlayerFSM;

    private void Start()
    {
        PlayerFSM = player.GetComponent<PlayerFSM>();

        // 데이터 초기화 
        DataManager.instance.playerdata.HeartCount = 3;
        DataManager.instance.playerdata.AttackCount = 0;

        Debug.Log("게임을 시작합니다.");
        Debug.Log("Heart = " + DataManager.instance.playerdata.HeartCount + "Attack = " + DataManager.instance.playerdata.AttackCount);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnElectric();
            HandleStartTrigger();
            
            StartCoroutine(NextScene());
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1.5f);

        SceneChange.instance.LoadNextScene();
    }

    void HandleStartTrigger()
    {
        PlayerFSM.isMoveable = false;
        PlayerFSM.moveX = 0;

        if (PlayerFSM.rb != null)
        {
            PlayerFSM.rb.velocity = Vector2.zero;
            Vector2 knockbackDir = new Vector2(-1, 1).normalized;
            float knockbackForce = 20f;
            PlayerFSM.rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }

        PlayerFSM.CollisionOff();
    }

    void OnElectric()
    {
        electric.SetActive(true);
    }
}
