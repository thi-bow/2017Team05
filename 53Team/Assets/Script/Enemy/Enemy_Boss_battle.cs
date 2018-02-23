using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enemy;
using UnityEngine.AI;

[Serializable]
public class Enemy_Boss_battle {

    public BattleType m_battleType;
    public WeponType m_currentWeponType;
    private WeponType m_currentMainWepon;
    private WeponType m_currentSubWepon;
    private Enemy_Boss_State m_base;
    public float m_dis;
    public float m_radius;
    private Vector3 m_targetPoint;
    private int m_currentPNum;
    private int m_moveNum;
    private float m_time;
    private float m_waitTime;
    private bool m_isEx;

    [Serializable]
    public struct Weapon
    {
        public weaponFire weapon;
        public float cool_time;
        public WeponType type;
    }
    public Weapon[] m_weapons;
    private float[] m_nextFire;
    public enum WeponType
    {
        normal_main,
        normal_sub,
        ex_main,
        ex_sub
    }

    public enum BattleType
    {
        move,
        fire,
        none
    }

    private readonly int        NORMAL_POS_NUM      = 3;
    private readonly float      NORMAL_WAIT_TIME    = 4.0f;
    private readonly int        EXTRA_POS_NUM       = 5;
    private readonly float      EXTRA_WAIT_TIME     = 3.0f;

    public void Init(Enemy_Boss_State aBase)
    {
        m_base = aBase;
        Array.Resize(ref m_nextFire, m_weapons.Length);

        m_currentWeponType = WeponType.normal_main;
        m_currentSubWepon = WeponType.normal_sub;
        m_waitTime = NORMAL_WAIT_TIME;
        m_moveNum = NORMAL_POS_NUM;
        m_isEx = false;

        var turrets = m_base.GetComponentsInChildren<Enemy_Turret>();
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].SetTarget(m_base.m_target);
        }

        ChangeType(BattleType.move);
    }

    public void Update()
    {
        switch (m_battleType)
        {
            case BattleType.move:
                Move();
                break;
            case BattleType.fire:
                m_time += Time.deltaTime;
                if (m_time >= m_waitTime)
                {
                    m_time = 0;
                    ChangeType(BattleType.move);
                }
                break;
            case BattleType.none:
                return;
            default:
                break;
        }


        //m_base.transform.LookAt(m_base.m_target);
        //var angle = m_base.transform.rotation.eulerAngles;
        //angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        //angle.x = Mathf.Clamp(angle.x, -25, 25);
        //m_base.transform.rotation = Quaternion.Euler(angle);

        CoolTime();
        Fire();
    }

    // 発狂モード
    public void ExMode()
    {
        if (m_isEx) return;

        Debug.LogWarning("Boss状態変化");
        m_currentMainWepon = WeponType.ex_main;
        m_currentSubWepon = WeponType.ex_sub;
        m_waitTime = EXTRA_WAIT_TIME;
        m_moveNum = EXTRA_POS_NUM;

        m_currentPNum = 0;

        m_time = 0;
        m_isEx = true;

        for (int i = 0; i < m_base.m_explostions.Length; i++)
        {
            m_base.m_explostions[i].Play();
            m_base.m_explostions[i].GetComponent<Enemy_Turret>().SetTarget(null);
        }

        ChangeType(BattleType.move);
    }

    public void ChangeType(BattleType aType)
    {
        m_battleType = aType;
        switch (m_battleType)
        {
            case BattleType.move:
                m_targetPoint = GetRandomPoint();
                m_currentWeponType = m_currentMainWepon;
                break;
            case BattleType.fire:
                m_currentWeponType = m_currentSubWepon;
                break;
            case BattleType.none:
                break;
            default:
                break;
        }
    }

    public bool IsWeaponType(WeponType aType)
    {
        return m_currentWeponType == aType;
    }

    public void Fire()
    {
        for (int i = 0; i < m_weapons.Length; i++)
        {
            if (IsWeaponType(m_weapons[i].type))
            {
                if (m_nextFire[i] <= 0 && !IsNull(m_weapons[i].weapon))
                {
                    m_nextFire[i] = m_weapons[i].cool_time;
                    m_weapons[i].weapon.fire();
                }
            }
        }
    }

    public void CoolTime()
    {
        for (int i = 0; i < m_weapons.Length; i++)
        {
            if (m_nextFire[i] > 0)
            {
                m_nextFire[i] -= Time.deltaTime;
            }
        }
    }

    // 設定されたルートを移動
    public void Move()
    {
        // if(m_base.GetComponent<NavMeshAgent>())
        if (m_base.IsArrival())
        {
            m_currentPNum++;
            if (m_currentPNum == m_moveNum)
            {
                m_currentPNum = 0;
                ChangeType(BattleType.fire);
                return;
            }
            else
            {
                m_targetPoint = GetRandomPoint();
            }
        }
        m_base.Move(m_targetPoint, m_isEx, m_base.m_target);
    }

    // 目標ポイントの設定
    private NavMeshHit m_navhit;
    public Vector3 GetRandomPoint()
    {
        Vector3 p;
        p.x = UnityEngine.Random.Range(-m_radius, m_radius);
        p.z = UnityEngine.Random.Range(-m_radius, m_radius);
        p.y = 0;

        p += m_base.m_target.position;

        NavMeshAgent agent = m_base.GetComponent<NavMeshAgent>();
        if(NavMesh.SamplePosition(p, out m_navhit, agent.radius * 4, 1))
        {
            // return m_navhit.position;

            float dis = Vector3.SqrMagnitude(m_navhit.position - m_base.m_target.position);
            // Debug.Log("kyori = " + dis + ":" + m_dis * m_dis);
            if (dis > m_dis * m_dis)
            {
                return m_navhit.position;
            }
            else
            {
                return GetRandomPoint();
            }
        }
        else
        {
            // Debug.DrawLine(m_base.transform.position, p, Color.red, 3f);
            return GetRandomPoint();
        }

        // return Vector3.zero;
    }

    static bool IsNull<T>(T obj) where T : class
    {
        var unityObj = obj as UnityEngine.Object;
        if (!object.ReferenceEquals(unityObj, null))
        {
            return unityObj == null;
        }
        else
        {
            return obj == null;
        }
    }    
}
