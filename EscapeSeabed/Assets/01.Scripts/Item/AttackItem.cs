using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DataManager.instance.playerdata.AttackCount++;
            Destroy(gameObject, 0.1f);
        }
    }

}
