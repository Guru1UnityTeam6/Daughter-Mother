using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ī�޶� ���� ���
    private GameObject target;
    // ī�޶� ���� �ӵ�
    public float moveSpeed;
    // ����� ���� ��ġ
    private Vector3 targetPosition;

    void Start()
    {
        // DontDestroyOnLoad(this.gameObject); // ���� ������Ʈ �ı����� -> �ε忡���� ���Ŵϱ�
        // 2��4�� ����
        target = GameObject.Find("Player");
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
        }
    }
}
