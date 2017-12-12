using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enemy;


[Serializable]
public class Enemy_Boss_battle {

    public BattleType m_battleType;
    public WeponType m_currentWeponType;
    private WeponType m_currentMainWepon;
    private WeponType m_currentSubWepon;
    private Enemy_Boss_State m_base;
    public float m_dis;
    public Vector3 m_currentPos;
    public Vector3 m_pos;
    private int m_currentPNum;
    private int m_moveNum;
    private float m_moveTime;
    private float m_waitTime;

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
    private readonly float      NORMAL_MOVE_TIME    = 2.5f;
    private readonly float      NORMAL_WAIT_TIME    = 4.0f;
    private readonly int        EXTRA_POS_NUM       = 5;
    private readonly float      EXTRA_MOVE_TIME     = 1.0f;
    private readonly float      EXTRA_WAIT_TIME     = 3.0f;

    public void Init(Enemy_Boss_State aBase)
    {
        m_base = aBase;
        Array.Resize(ref m_nextFire, m_weapons.Length);

        m_currentWeponType = WeponType.normal_main;
        m_currentSubWepon = WeponType.normal_sub;
        m_moveTime = NORMAL_MOVE_TIME;
        m_waitTime = NORMAL_WAIT_TIME;
        m_moveNum = NORMAL_POS_NUM;

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
                time += Time.deltaTime;
                if (time >= m_waitTime)
                {
                    time = 0;
                    ChangeType(BattleType.move);
                }
                break;
            case BattleType.none:
                return;
            default:
                break;
        }


        m_base.transform.LookAt(m_base.m_target);
        var angle = m_base.transform.rotation.eulerAngles;
        angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        angle.x = Mathf.Clamp(angle.x, -25, 25);
        m_base.transform.rotation = Quaternion.Euler(angle);

        CoolTime();
        Fire();
    }

    // 発狂モード
    public void ExMode()
    {
        Debug.LogWarning("Boss状態変化");
        m_currentMainWepon = WeponType.ex_main;
        m_currentSubWepon = WeponType.ex_sub;
        m_moveTime = EXTRA_MOVE_TIME;
        m_waitTime = EXTRA_WAIT_TIME;
        m_moveNum = EXTRA_POS_NUM;

        m_currentPNum = 0;

        time = 0;

        ChangeType(BattleType.move);
    }

    public void ChangeType(BattleType aType)
    {
        m_battleType = aType;
        switch (m_battleType)
        {
            case BattleType.move:
                SetRoot();
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
                if (m_nextFire[i] == 0 && !IsNull(m_weapons[i].weapon))
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
            if(m_nextFire[i] > 0)
            {
                m_nextFire[i] = Mathf.Max(0, m_nextFire[i] - 0.01f);
            }
        }
    }
        // 移動ルートの設定
    public void SetRoot()
    {

        m_currentPos = m_base.transform.position;

        SetTargetPostion();
        m_pos = pos;
    }

    // 設定されたルートを移動
    private float time;
    public void Move()
    {
        time += Time.deltaTime / m_moveTime;
        m_base.transform.position = Vector3.Lerp(m_currentPos, m_pos, time);
        if(time >= 1)
        {
            m_currentPNum++;
            time = 0;
            if(m_currentPNum == m_moveNum)
            {
                m_currentPNum = 0;
                ChangeType(BattleType.fire);
                return;
            }
            SetRoot();
        }
    }

    // 目標ポイントの設定
    private Transform t;
    private Vector3 pos;
    private Ray ray;
    public void SetTargetPostion()
    {
        t = m_base.m_target;

        // 目標高度の設定
        pos.y = t.position.y + UnityEngine.Random.Range(1f, 5f);

        // 目標位置の設定
        pos.x = t.position.x + UnityEngine.Random.Range(-m_dis, m_dis);
        pos.z = t.position.z + UnityEngine.Random.Range(-m_dis, m_dis);

        ray = new Ray(m_currentPos, pos - m_currentPos);
        if(Physics.SphereCast(ray, 5.0f))
        {
            SetTargetPostion();
        }
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
