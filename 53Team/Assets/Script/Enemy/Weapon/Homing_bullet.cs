using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_bullet : Enemy_bullet {

    [Space(10)]
    public Transform m_target;
    public float m_speed = 5f;
    public float m_turnSpeed = 3f;

    private Rigidbody m_rd;

    protected override IEnumerator Start()
    {
        m_rd = GetComponent<Rigidbody>();
        return base.Start();
    }

    void LateUpdate () {
        if (m_target == null)
            return;

        // ここら辺に誘導処理
        Vector3 dir = (m_target.position - transform.position);
        Quaternion vec = Quaternion.LookRotation(dir, transform.up);
        // Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, m_turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, vec, m_turnSpeed * Time.deltaTime);
        m_rd.velocity = transform.forward * m_speed;
    }
}
