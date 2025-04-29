using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;   // �̱���

    public List<Image> heartImages;   // 3��
    public List<Image> attackImages;  // 6��

    public Sprite heartOnSprite;      // Ȱ��ȭ ��������Ʈ
    public Sprite heartOffSprite;     // ��Ȱ��ȭ ��������Ʈ

    public Sprite attackOnSprite;
    public Sprite attackOffSprite;

    private int previousHeartCount = -1;
    private int previousAttackCount = -1;

    Color darkGray = new Color(0.3f, 0.3f, 0.3f, 1f);

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

    void Update()
    {
        // ���� �÷��̾� ������ �б�
        int currentHeart = Mathf.Clamp(DataManager.instance.playerdata.HeartCount, 0, heartImages.Count);
        int currentAttack = Mathf.Clamp(DataManager.instance.playerdata.AttackCount, 0, attackImages.Count);

        // ������ DataManager �����͵� ���� ���� (Ȥ�� �� �Ǽ� ���)
        DataManager.instance.playerdata.HeartCount = currentHeart;
        DataManager.instance.playerdata.AttackCount = currentAttack;

        // Heart�� ����Ǿ��� ��
        if (currentHeart != previousHeartCount)
        {
            // �����ߴٸ� �����̱�
            if (currentHeart < previousHeartCount)
            {
                StartCoroutine(FlashHeart(previousHeartCount - 1)); // �پ�� �κи� ����
            }
            UpdateHeartUI(currentHeart);
            previousHeartCount = currentHeart;
        }

        // Attack�� ����Ǿ��� ��
        if (currentAttack != previousAttackCount)
        {
            // �����ߴٸ� �����̱�
            if (currentAttack > previousAttackCount)
            {
                StartCoroutine(FlashAttack(currentAttack - 1)); // ���� ���� �κи� ����
            }
            UpdateAttackUI(currentAttack);
            previousAttackCount = currentAttack;
        }
    }

    void UpdateHeartUI(int currentHeart)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHeart)
            {
                heartImages[i].sprite = heartOnSprite;
                heartImages[i].color = Color.white; // Ȱ��ȭ - ���
            }
            else
            {
                heartImages[i].sprite = heartOffSprite;
                heartImages[i].color = darkGray; // ��Ȱ��ȭ - ȸ��
            }
        }
    }

    void UpdateAttackUI(int currentAttack)
    {
        for (int i = 0; i < attackImages.Count; i++)
        {
            if (i < currentAttack)
            {
                attackImages[i].sprite = attackOnSprite;
                attackImages[i].color = Color.white; // Ȱ��ȭ - ���
            }
            else
            {
                attackImages[i].sprite = attackOffSprite;
                attackImages[i].color = darkGray; // ��Ȱ��ȭ - ȸ��
            }
        }
    }

    // Heart�� �پ����� �� �����̴� �ڷ�ƾ
    IEnumerator FlashHeart(int index)
    {
        if (index < 0 || index >= heartImages.Count)
            yield break;

        Image img = heartImages[index];
        Color originalColor = img.color;

        for (int i = 0; i < 3; i++) // 3�� ����
        {
            img.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            img.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        UpdateHeartUI(DataManager.instance.playerdata.HeartCount);
    }

    // Attack�� �þ�� �� �����̴� �ڷ�ƾ
    IEnumerator FlashAttack(int index)
    {
        if (index < 0 || index >= attackImages.Count)
            yield break;

        Image img = attackImages[index];
        Color originalColor = img.color;

        for (int i = 0; i < 3; i++) // 3�� ����
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            img.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        UpdateAttackUI(DataManager.instance.playerdata.AttackCount);
    }
}
