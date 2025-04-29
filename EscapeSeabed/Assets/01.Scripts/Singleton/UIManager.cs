using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;   // 싱글톤

    public List<Image> heartImages;   // 3개
    public List<Image> attackImages;  // 6개

    public Sprite heartOnSprite;      // 활성화 스프라이트
    public Sprite heartOffSprite;     // 비활성화 스프라이트

    public Sprite attackOnSprite;
    public Sprite attackOffSprite;

    private int previousHeartCount = -1;
    private int previousAttackCount = -1;

    Color darkGray = new Color(0.3f, 0.3f, 0.3f, 1f);

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

    void Update()
    {
        // 현재 플레이어 데이터 읽기
        int currentHeart = Mathf.Clamp(DataManager.instance.playerdata.HeartCount, 0, heartImages.Count);
        int currentAttack = Mathf.Clamp(DataManager.instance.playerdata.AttackCount, 0, attackImages.Count);

        // 강제로 DataManager 데이터도 범위 고정 (혹시 모를 실수 대비)
        DataManager.instance.playerdata.HeartCount = currentHeart;
        DataManager.instance.playerdata.AttackCount = currentAttack;

        // Heart가 변경되었을 때
        if (currentHeart != previousHeartCount)
        {
            // 감소했다면 깜빡이기
            if (currentHeart < previousHeartCount)
            {
                StartCoroutine(FlashHeart(previousHeartCount - 1)); // 줄어든 부분만 깜빡
            }
            UpdateHeartUI(currentHeart);
            previousHeartCount = currentHeart;
        }

        // Attack이 변경되었을 때
        if (currentAttack != previousAttackCount)
        {
            // 증가했다면 깜빡이기
            if (currentAttack > previousAttackCount)
            {
                StartCoroutine(FlashAttack(currentAttack - 1)); // 새로 켜진 부분만 깜빡
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
                heartImages[i].color = Color.white; // 활성화 - 흰색
            }
            else
            {
                heartImages[i].sprite = heartOffSprite;
                heartImages[i].color = darkGray; // 비활성화 - 회색
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
                attackImages[i].color = Color.white; // 활성화 - 흰색
            }
            else
            {
                attackImages[i].sprite = attackOffSprite;
                attackImages[i].color = darkGray; // 비활성화 - 회색
            }
        }
    }

    // Heart가 줄어들었을 때 깜빡이는 코루틴
    IEnumerator FlashHeart(int index)
    {
        if (index < 0 || index >= heartImages.Count)
            yield break;

        Image img = heartImages[index];
        Color originalColor = img.color;

        for (int i = 0; i < 3; i++) // 3번 깜빡
        {
            img.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            img.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        UpdateHeartUI(DataManager.instance.playerdata.HeartCount);
    }

    // Attack이 늘어났을 때 깜빡이는 코루틴
    IEnumerator FlashAttack(int index)
    {
        if (index < 0 || index >= attackImages.Count)
            yield break;

        Image img = attackImages[index];
        Color originalColor = img.color;

        for (int i = 0; i < 3; i++) // 3번 깜빡
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            img.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }

        UpdateAttackUI(DataManager.instance.playerdata.AttackCount);
    }
}
