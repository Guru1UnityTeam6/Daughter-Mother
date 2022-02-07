using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmManager : MonoBehaviour
{
    public static BgmManager instance;  //�̱���ȭ��Ŵ=>�̰� ���� �Ѿ�� �ı��Ǹ� �ȵǱ� ����

    public AudioClip[] clips; // ������ǵ�

    private AudioSource source;

    //���ʹ��� ���ʹ̹��� ������ true�� ���� ����� ������ ����
    public Enemy1Controller ecbgm1;
    public Enemy2Controller ecbgm2;
    public Enemy3Controller ecbgm3;

    private void Awake()
    {
        // DontDestroy ����
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
        // �ҽ� �ʱ�ȭ
        source = GetComponent<AudioSource>();
        // playerMusicTrack �ʱ�ȭ
        Play(0);
    }

    // ������ �����Ű�� �Լ�
    public void Play(int _playMusicTrack)
    {
        // ũ�� ����
        source.volume = 1f;
        if (_playMusicTrack == 1)
        {
            source.volume = 0.7f;
        }
        //_playMusicTrack ������� ������ �ֱ� ������ ���° ���� ������ ����
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
