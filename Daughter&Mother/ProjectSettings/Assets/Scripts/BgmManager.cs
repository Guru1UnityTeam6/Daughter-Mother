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

        //���ӿ��� ���� �� ���ӿ����뷡�� �ٲٱ�
        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            source.clip = clips[2]; //Ŭ���̶� �迭�� �ִ� 2���� ���ӿ��� ��� ���
            source.Play();
        }
        //�������� �� ���� ������� �ٲٱ�
        if(SceneManager.GetActiveScene().name == "Enemy1" || SceneManager.GetActiveScene().name == "Enemy2" || SceneManager.GetActiveScene().name == "Enemy3")
        {
            //���ʹ� ������ true�϶��� ���
            if ((ecbgm1.enemyMoving == true) || (ecbgm2.enemyMoving == true) || (ecbgm2.enemyMoving == true))
            {
                source.clip = clips[1]; //Ŭ���̶� �迭�� �ִ� 1���� ���� ��� ���
                source.Play();
                
                //�׸��� ���ʹ��� �ǰ� 0�� �Ǹ�  �޸� ���� ��� �� �ٽ� ������� ���
                //���� ����
                if (ecbgm1.hp <= 0)
                {
                    source.clip = clips[0];
                    source.Play();
                }
            }
        }
    }
    public void Stop()
    {
        source.Stop();
    }
}
