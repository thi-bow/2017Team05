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
    private Enemy_Boss_State m_base;
    public float m_dis;
    private Vector3[] m_currentPos = new Vector3[3];
    private Vector3[] m_poss = new Vector3[3];
    private int m_currentPNum;

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
        main,
        sub
    }

    public enum BattleType
    {
        move,
        fire
    }

    private readonly int MAX_POS_NUM = 3;

    public void Init(Enemy_Boss_State aBase)
    {
        m_base = aBase;
        Array.Resize(ref m_nextFire, m_weapons.Length);
        ChangeType(BattleType.move);
    }

    public void Update()
    {
        m_base.transform.LookAt(m_base.m_target);
        var angle = m_base.transform.rotation.eulerAngles;
        angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        angle.x = Mathf.Clamp(angle.x, -25, 25);
        m_base.transform.rotation = Quaternion.Euler(angle);

        CoolTime();
        Fire();
        switch (m_battleType)
        {
            case BattleType.move:
                Move();               
                break;
            case BattleType.fire:
                time += Time.deltaTime;
                if (time >= 3)
                {
                    time = 0;
                    ChangeType(BattleType.move);
                }
                break;
            default:
                break;
        }
    }

    public void End()
    {

    }

    public void ChangeType(BattleType aType)
    {
        m_battleType = aType;
        switch (m_battleType)
        {
            case BattleType.move:
                SetRoot();
                m_currentWeponType = WeponType.main;
                break;
            case BattleType.fire:
                m_currentWeponType = WeponType.sub;
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
                if (m_nextFire[i] == 0)
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

        m_currentPos[0] = m_base.transform.position;

        for (int i = 0; i < MAX_POS_NUM; i++)
        {
            SetTargetPostion();
            m_poss[i] = pos;
            if (i + 1 < MAX_POS_NUM) m_currentPos[i + 1] = pos;
        }
    }

    // 設定されたルートを移動
    private float time;
    public void Move()
    {
        time += Time.deltaTime;
        m_base.transform.position = Vector3.Lerp(m_currentPos[m_currentPNum], m_poss[m_currentPNum], time);
        if(time >= 1)
        {
            m_currentPNum++;
            time = 0;
            if(m_currentPNum == MAX_POS_NUM)
            {
                m_currentPNum = 0;
                ChangeType(BattleType.fire);
            }
        }
    }

    // 目標ポイントの設定
    private Transform t;
    private Vector3 pos;
    public void SetTargetPostion()
    {
        t = m_base.m_target;

        // 目標高度の設定
        pos.y = t.position.y + UnityEngine.Random.Range(1f, 5f);

        // 目標位置の設定
        pos.x = t.position.x + UnityEngine.Random.Range(-m_dis, m_dis);
        pos.z = t.position.z + UnityEngine.Random.Range(-m_dis, m_dis);
    }
}
