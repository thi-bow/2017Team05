using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

[RequireComponent(typeof(ApproachAttack))]
public class Enemy_Standard_battle_behavior {

    private Enemy_Standard m_enemy;
    private ApproachAttack m_attack;

    private int m_time = 0;
    private int m_cooltime = 1;

    public Enemy_Standard_battle_behavior(Enemy_Standard aEnemy)
    {
        m_enemy = aEnemy;
        m_attack = aEnemy.GetComponent<ApproachAttack>();
        ResetTime();
    }

    public void UpdateBattleState()
    {
        var vec = m_enemy.m_target.position - m_enemy.m_viewPoint.position;
        var dis = m_enemy.GetSqrDistance();
        if (m_enemy.IsInDistance(vec, 3))
        {
            m_time--;

            if (m_time <= 0)
            {
                ResetTime();
                PhysicalAttack();
            }
        }
        else
        {
            //m_enemy.RightShot(vec);
            m_enemy.LeftShot(vec);
        }

    }

    public void PhysicalAttack()
    {
        Debug.Log("ﾁｪｽﾄｫｫｫｫ!!");
        m_attack.Approach(m_enemy._charaPara._rightAttack);
    }

    public void ResetTime()
    {
        m_time = m_cooltime * 60;
    }

}
