using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy2Fire : MonoBehaviour
{
    // �ް� ���� ������ ����
    public GameObject DalgonaFactory;
    // �߻� ��ġ
    public GameObject firePosition; 

    //����ð�
    float currentTime;
    //�����ð�
    public float createTime = 5;
    // chatManager
    ChatManager chatManager;

    private void Start() 
    {
        chatManager = FindObjectOfType<ChatManager>();
    }

    void Update()
    {
        Enemy2Controller ec2 = GameObject.Find("Enemy2").GetComponent<Enemy2Controller>();
        Enemy3Controller ec3 = GameObject.Find("Enemy3").GetComponent<Enemy3Controller>();
        if (SceneManager.GetActiveScene().name == "Enemy2")
        {
            if (ec2.enemyMoving == true)
            {
                // �κ��丮 ������ �� ���� ����
                if ((chatManager.isAction) ||(Inventory.instance.activeInventory == false))
                {
                    //1.�ð��� �帣�ٰ�
                    currentTime += Time.deltaTime;
                    //2.���� ����ð��� �����ð��� �Ǹ�
                    if (currentTime > createTime)
                    {
                        //�Ѿ� ���忡�� �Ѿ��� �����.
                        GameObject bullet = Instantiate(DalgonaFactory);
                        //�Ѿ��� �߻��Ѵ�
                        bullet.transform.position = firePosition.transform.position;
                        //����ð��� 0���� �ʱ�ȭ
                        currentTime = 0;
                    }
                }
            }
            else
            {
                ec2.enemyMoving = false;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Enemy3")
        {
            if (ec3.enemyMoving == true)
            {
                // �κ��丮 ������ �� ���� ����
                if (Inventory.instance.activeInventory == false)
                {
                    //1.�ð��� �帣�ٰ�
                    currentTime += Time.deltaTime;
                    //2.���� ����ð��� �����ð��� �Ǹ�
                    if (currentTime > createTime)
                    {
                        //�Ѿ� ���忡�� �Ѿ��� �����.
                        GameObject bullet = Instantiate(DalgonaFactory);
                        //�Ѿ��� �߻��Ѵ�
                        bullet.transform.position = firePosition.transform.position;
                        //����ð��� 0���� �ʱ�ȭ
                        currentTime = 0;
                    }
                }
            }
            else
            {
                ec3.enemyMoving = false;
            }
        }
    }
}
