using UnityEngine;


// BGM ���
// SoundManager.instance.PlayBGM(menuBgmClip);

// �ٸ� ����������� ��ȯ
// SoundManager.instance.PlayBGM(playBgmClip);

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioClip bgmClip;

    private void Awake()
    {
        // �̱��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // ������� ���
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = volume;
    }
    */
}