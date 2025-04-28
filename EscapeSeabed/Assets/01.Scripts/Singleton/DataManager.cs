using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;   // ΩÃ±€≈Ê

    public PlayerData playerdata = new PlayerData();

    private void Awake()
    {
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

    void Start()
    {
        // √ ±‚»≠ 
        playerdata.HeartCount = 2;
        playerdata.AttackCount = 1;
    }

    void Update()
    {
        
    }
}

public class PlayerData
{
    public int HeartCount;  // DataManager.instance.playerdata.HeartCount--;
    public int AttackCount;
}