using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    void Update()
    {
        // ���� �ð����� �ް� ������ �߻��ϰ� �ʹ�.
        // (if��)
        // �ް� ���� ���忡�� �ް� ������ �����.

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
