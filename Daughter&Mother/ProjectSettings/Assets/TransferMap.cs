using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement; //Scene �Ŵ��� ���̺귯�� �߰�

//public class TransferMap : MonoBehaviour
//{
//    public string transferMapName; // �̵��� ���̸�

//    // Start is called before the first frame update
//    void Start()
//    {
//        DontDestroyOnLoad(this.gameObject); // ���� ������Ʈ �ı�����

//        // �ִϸ����� ������Ʈ ��������
//        boxCollider = GetComponent<BoxCollider2D>();
//        animator = GetComponent<Animator>();
//    }

//    // �ڽ� �ݶ��̴��� ��� ���� �̺�Ʈ �߻�
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.name == "Player")
//        {
//            thePlayer.currentMapName = transferMapName;
//            //SceneManager.LoadScene(transferMapName);
//            theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
//            thePlayer.transform.position = target.transform.position;

//        }
//    }
//}
