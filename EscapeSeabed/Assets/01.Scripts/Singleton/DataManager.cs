using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;   // �̱���

    public PlayerData playerdata = new PlayerData();

    private void Awake()
    {
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

    void Start()
    {
        // ������ �ʱ�ȭ 
        playerdata.HeartCount = 3;
        playerdata.AttackCount = 0;
    }
}

public class PlayerData
{
    public int HeartCount;  // DataManager.instance.playerdata.HeartCount--;
    public int AttackCount; // DataManager.instance.playerdata.AttackCount++;
}