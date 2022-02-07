using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    // ī�޶� ���� ���
    private GameObject target;
    // ī�޶� ���� �ӵ�
    public float moveSpeed;
    // ����� ���� ��ġ
    private Vector3 targetPosition;


    //�ε� �� �Ÿ������� �۵��ϰ� �߰�
    public BoxCollider2D bound;//���� ������ ��
    public BoxCollider2D boundCenter; //�߰������� ������ ī�޶��� y��ǥ -10�� ��
    //�ٿ�� �������� �ּ� �ִ� xyz���� ����.
    private Vector3 minBound;
    private Vector3 maxBound;
    //ī�޶� �߽��� �̿��� �����̱� ������ ī�޶��� �ݳʺ�, �ݳ��̸� ���� ����
    //�̰� �������� ���� ��� 
    private float halfWidth;
    private float halfHeight;
    //ī�޶��� �ݳ��̰��� ���� �Ӽ��� �̿��ϱ� ���� ����.
    private Camera theCamera;
    
    //�߾ӿ� �ִ� boundCenter�� ������ ī�޶��� ���� y�� ��ǥ ����

    void Start()
    {
        target = GameObject.Find("Player");
        // DontDestroyOnLoad(this.gameObject); // ���� ������Ʈ �ı����� -> �ε忡���� ���Ŵϱ�
        // 2��4�� ����
        //target = GameObject.Find("Player");
        if (SceneManager.GetActiveScene().name == "Road")
        {
            Debug.Log("�ε�� Ȯ��");
            theCamera = GetComponent<Camera>();
            minBound = bound.bounds.min;    //bound������ ������ min�̶�� �ּڰ��� �ְڴٴ� ��
            maxBound = bound.bounds.max;
            halfHeight = theCamera.orthographicSize;  //ī�޶��� �ݳ���  ī�޶� ������:5  
            halfWidth = halfHeight * Screen.width / Screen.height;
        }
    }

    void Update()
    {
        // ����� �ִ��� üũ
        if (target.gameObject != null)
        {
            // �÷��̾��� ��ġ�� ����
            // this�� ī�޶� �ǹ� (z���� ī�޶��� �״�� ����) 
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            
            // vectorA -> B���� T�� �ӵ��� �̵�
            // Ÿ���� ��ġ��... �� ī�޶��� �߽��� �÷��̾�
            // 1�ʿ� movespeed ��ŭ �̵�
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            if(SceneManager.GetActiveScene().name == "Road")
            {
                float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
                float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
                this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);

                if((target.transform.position.y <= 10) && (target.transform.position.x <= 18) && (target.transform.position.x >= 13))
                {
                    void OnTriggerEnter2D(Collider2D collision)
                    {
                        print("�߾� �浹");
                        this.transform.position = new Vector3(clampedX, -10, this.transform.position.z);
                    }
                    OnTriggerEnter2D(boundCenter);
                }
            }
        }
    }
}
