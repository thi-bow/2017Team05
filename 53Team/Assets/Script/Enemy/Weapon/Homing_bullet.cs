using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Homing_bullet : Enemy_bullet {

    [Space(10)]
    public float m_speed = 5f;
    public float m_turnSpeed = 3f;
    public float m_waitTime = 0.3f;
    public bool m_homing = false;

    private Transform m_target;
    private Vector3 m_point;
    private Rigidbody m_rd;

    private readonly Vector3 vector3Zero = new Vector3(0, 0, 0);

    protected override IEnumerator Start()
    {
        m_rd = GetComponent<Rigidbody>();
        return base.Start();
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
        StartCoroutine(HomingStart());
    }

    public void SetTarget(Vector3 target)
    {
        m_target = null;
        m_point = target;
        StartCoroutine(HomingStart());
    }

    IEnumerator HomingStart()
    {
        yield return new WaitForSeconds(m_waitTime);
        m_homing = true;
    }

    void LateUpdate () {
        if (!m_homing)
            return;

        if (m_target != null)
            m_point = m_target.position;

        // ここら辺に誘導処理
        Vector3 dir = (m_point - transform.position).normalized;

        float step = m_turnSpeed * Time.deltaTime;
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle <= 15)
            m_turnSpeed = 0.5f;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);

        m_rd.velocity = transform.forward * m_speed;
    }

    protected override void OnTriggerEnter(Collider col)
    {
        m_homing = false;
        base.OnTriggerEnter(col);
    }
}
