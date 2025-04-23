using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject BulletPrefab;

    public float launchSpeed = 10.0f; // �߻�Ǵ� ������ �ӵ�

    void Update()
    {
        // [ �� �߻� ]
        // �÷��̾� �ִϸ��̼� !! 
        // jump Ȥ�� idle ���̸� isShooting bool�Ķ���� true
        // run ���̸� isSootingRun bool�Ķ���� true
        // 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���������κ��� ���ο� �̻��� ���� ������Ʈ ����
            GameObject missile = Instantiate(BulletPrefab, transform.position, transform.rotation);

            // �̻��Ϸκ��� ������ٵ� 2D ������Ʈ ������
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();

            // �̻����� �������� �߻�
            rb.AddForce(transform.right * launchSpeed, ForceMode2D.Impulse);
        }

    }
}
