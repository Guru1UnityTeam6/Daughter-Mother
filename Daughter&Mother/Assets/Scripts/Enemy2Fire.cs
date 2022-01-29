using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Fire : MonoBehaviour
{
    // �ް� ���� ������ ����
    public GameObject DalgonaFactory;

    // �߻� ��ġ
    public GameObject firePosition;

    void Update()
    {
        // ���� �ð����� �ް� ������ �߻��ϰ� �ʹ�.
        // (if��)
        // �ް� ���� ���忡�� �ް� ������ �����.
        if(Input.GetKey(KeyCode.F))
        {
            GameObject DalgonaPiece = Instantiate(DalgonaFactory);

            // �ް� ������ �߻��Ѵ�.(�ް� ������ �߻� ��ġ�� ������ ����)
            DalgonaPiece.transform.position = firePosition.transform.position;
        }
    }
}
