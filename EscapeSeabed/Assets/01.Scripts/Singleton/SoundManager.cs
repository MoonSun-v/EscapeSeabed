using UnityEngine;


// BGM 재생
// SoundManager.instance.PlayBGM(menuBgmClip);

// 다른 배경음악으로 전환
// SoundManager.instance.PlayBGM(playBgmClip);

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioClip bgmClip;

    private void Awake()
    {
        // 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // 배경음악 재생
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