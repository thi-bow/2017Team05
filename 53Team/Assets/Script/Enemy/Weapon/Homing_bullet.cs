using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_bullet : Enemy_bullet {

    [Space(10)]
    public Transform m_target;
    public float m_rotateSpeed;

    private Rigidbody m_rd;

    protected override IEnumerator Start()
    {
        m_rd = GetComponent<Rigidbody>();
        return base.Start();
    }

    void Update () {
        if (m_target == null)
            return;

        // ここら辺に誘導処理

    }
}
