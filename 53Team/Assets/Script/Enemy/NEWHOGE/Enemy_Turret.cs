using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turret : MonoBehaviour {

    [Space(10)]
    public Transform m_horizontalAxis;
    public Transform m_verticalAxis;

    public float m_range = 10f;
    public float m_turnSpeed = 50;
    public float m_horizontalConstraint = 90;
    public float m_upConstraint = 20;
    public float m_downConstraint = -10;


    public Weapon[] m_weapons;

    private float[] m_nextFire;
    public Vector3 m_offset;
    private Vector3 m_dirToTarget;
    private Vector3 m_forwardXZ, forwardYZ;
    private Vector3 m_dirXZ, dirYZ;
    private Quaternion m_horizontalAxis_v, m_horizontalAxis_def;
    private Quaternion m_newRotationX, m_newRotationY;
    private Quaternion m_original, m_originalBarrel;

    [Serializable]
    public struct Weapon
    {
        public Transform weapon;
        public float cool_time;
    }

    public bool IsOutRange { get; private set; }
    public Transform Target { get; private set; }

    private void Start()
    {
        m_originalBarrel = m_verticalAxis.transform.rotation;
        m_horizontalAxis_v = m_horizontalAxis.transform.rotation;
        m_horizontalAxis_def = m_horizontalAxis_v;
        m_original = Quaternion.Euler(m_horizontalAxis_v.eulerAngles.x, 0, 0);

        Array.Resize(ref m_nextFire, m_weapons.Length);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < m_weapons.Length; i++)
        {
            if(m_nextFire[i] > 0)
            {
                m_nextFire[i] -= Time.deltaTime;
            }
        }

        Aim(Target);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void SetOffset(Vector3 offset)
    {
        m_offset = offset;
    }

    public void Aim(Transform target)
    {
        if (!target)
        {
            m_horizontalAxis_v = Quaternion.RotateTowards(m_horizontalAxis_v, m_horizontalAxis.transform.rotation, m_turnSpeed / 10);
            m_horizontalAxis.rotation = m_horizontalAxis_v;

            return;
        }

        Aim(target.position);
    }

    public void Aim(Vector3 target)
    {
        if (m_offset != Vector3.zero)
            target += m_offset;

        m_dirToTarget = (target - m_horizontalAxis.transform.position);
        Debug.DrawRay(m_horizontalAxis.position, m_dirToTarget, Color.blue);

        Vector3 originalForward = new Vector3(0, 0, 1);//Vector3.up;// original *
        Vector3 yAxis = Vector3.up; // world y axis
        m_dirXZ = Vector3.ProjectOnPlane(m_dirToTarget, yAxis);

        m_forwardXZ = Vector3.ProjectOnPlane(originalForward, yAxis);
        float yAngle = Vector3.Angle(m_dirXZ, m_forwardXZ) * Mathf.Sign(Vector3.Dot(yAxis, Vector3.Cross(m_forwardXZ, m_dirXZ)));
        float parentY = transform.rotation.eulerAngles.y;
        if (parentY < 0) parentY = -parentY;
        if (parentY > 180) parentY -= 360;
        float yClamped = Mathf.Clamp(yAngle, -m_horizontalConstraint + parentY, m_horizontalConstraint + parentY);
        Quaternion yRotation = Quaternion.AngleAxis(yClamped, Vector3.up);

        Quaternion xRotation = Quaternion.Euler(0, 0, 0);
        float xClamped = 0;
        float xAngle = 0;
        if (yClamped == yAngle)
        {
            m_dirToTarget = (target - m_verticalAxis.transform.position);
            originalForward = yRotation * new Vector3(0, 0, 1);
            Vector3 xAxis = yRotation * Vector3.right; // our local x axis
            dirYZ = Vector3.ProjectOnPlane(m_dirToTarget, xAxis);
            forwardYZ = Vector3.ProjectOnPlane(originalForward, xAxis);
            xAngle = Vector3.Angle(dirYZ, forwardYZ) * Mathf.Sign(Vector3.Dot(xAxis, Vector3.Cross(forwardYZ, dirYZ)));
            xClamped = Mathf.Clamp(xAngle, -m_upConstraint, -m_downConstraint);
            xRotation = Quaternion.AngleAxis(xClamped, Vector3.right);
        }


        if (m_horizontalAxis.transform == m_verticalAxis.transform)
        {
            m_newRotationX = yRotation * m_original * xRotation;
            m_horizontalAxis_v = Quaternion.RotateTowards(m_horizontalAxis_v, m_newRotationX, m_turnSpeed / 10);
        }
        else
        {
            m_newRotationX = yRotation * m_original;
            m_horizontalAxis_v = Quaternion.RotateTowards(m_horizontalAxis_v, m_newRotationX, m_turnSpeed / 10);
            m_newRotationY = m_originalBarrel * xRotation;
            m_verticalAxis.localRotation = Quaternion.RotateTowards(m_verticalAxis.localRotation, m_newRotationY, m_turnSpeed / 10);
        }
        m_horizontalAxis.rotation = m_horizontalAxis_v;

        IsOutRange = xClamped == xAngle && yClamped == yAngle && m_dirToTarget.sqrMagnitude <= m_range * m_range && m_horizontalAxis_v.eulerAngles == m_newRotationX.eulerAngles;
    }

    public void Fire()
    {
        for (int i = 0; i < m_weapons.Length; i++)
        {
            if(m_nextFire[i] <= 0)
            {
                m_nextFire[i] = m_weapons[i].cool_time;
                var weapnAnim = m_weapons[i].weapon.GetComponent<Animator>();
                weapnAnim.SetBool("Fire", true);
            }
        }
    }
}
