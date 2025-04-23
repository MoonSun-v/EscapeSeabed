using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject BulletPrefab;

    public float launchSpeed = 10.0f; // 발사되는 순간의 속도

    void Update()
    {
        // [ 총 발사 ]
        // 플레이어 애니메이션 !! 
        // jump 혹은 idle 중이면 isShooting bool파라미터 true
        // run 중이면 isSootingRun bool파라미터 true
        // 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 프리팹으로부터 새로운 미사일 게임 오브젝트 생성
            GameObject missile = Instantiate(BulletPrefab, transform.position, transform.rotation);

            // 미사일로부터 리지드바디 2D 컴포넌트 가져옴
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();

            // 미사일을 전방으로 발사
            rb.AddForce(transform.right * launchSpeed, ForceMode2D.Impulse);
        }

    }
}
