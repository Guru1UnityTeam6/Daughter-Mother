//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StartPoint : MonoBehaviour
//{
//    public string startPoint; // �̵��Ǿ�� ���̸��� üũ�ϱ� ���� ����
//    private MovingObject thePlayer; // ĳ���� ��ü �������� ���� ����
//    private CamaraManager theCamera; // �ڿ������� ī�޶� �̵��� ���� ������ ī�޶� ����

//    void Start()
//    {
//        theCamera = FindObjectOfType<CamaraManager>(); // ī�޶� ������ ī�޶� ��ü�� �Ҵ�
//        thePlayer = FindObjectOfType<MovingObject>(); // ĳ���� ������ ���� ĳ���� ��ü�� �Ҵ�


//        if (startPoint == thePlayer.currentMapName)
//        {
//            // ī�޶� �̵�
//            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
//            // ĳ���� �̵�
//            thePlayer.transform.position = this.transform.position;
//        }
//    }

//    void Update()
//    {

//    }
//}
