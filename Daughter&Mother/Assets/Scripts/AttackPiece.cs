using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPiece : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 3;

    void Update()
    {
        // 1. ������ ���Ѵ�.
        Vector3 dir = Vector3.up;
        // 2. �̵��ϰ� �ʹ�. ���� P = P0 + vt
        transform.position += dir * speed * Time.deltaTime;
    }
}
