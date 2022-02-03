using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    static public BgmManager instance;  //�̱���ȭ��Ŵ=>�̰� ���� �Ѿ�� �ı��Ǹ� �ȵǱ� ����

    public AudioClip[] clips; // ������ǵ�

    private AudioSource source;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }


    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack)
    { //_playMusicTrack ������� ������ �ֱ� ������ ���° ���� ������ ����
        source.volume = 1f;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
}
