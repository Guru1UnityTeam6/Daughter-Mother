using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPoint : MonoBehaviour
{
    // �� �̵� �� ������ ������ �̸�
    public string doorPoint;

    void Start()
    {
        if (doorPoint == PlayerController.instance.currentDoor)
        {
            // �÷��̾��� ��ġ�� doorPoint�� ����
            PlayerController.instance.transform.position = this.transform.position;
        }
    }
}
