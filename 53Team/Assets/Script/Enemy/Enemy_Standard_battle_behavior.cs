using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class Enemy_Standard_battle_behavior {

    private Enemy_Standard m_enemy;

    private int m_time = 0;
    private int m_cooltime = 1;

    public Enemy_Standard_battle_behavior(Enemy_Standard aEnemy)
    {
        m_enemy = aEnemy;
        ResetTime();
    }

    public void UpdateBattleState()
    {
        var vec = m_enemy.m_target.position - m_enemy.m_viewPoint.position;
        var dis = m_enemy.GetSqrDistance();
        if (m_enemy.IsInDistance(vec, 5))
        {
            m_time--;

            if (m_time <= 0)
            {
                PhysicalAttack();
                ResetTime();
            }
        }
        else
        {
            //m_enemy.RightShot(vec);
            //m_enemy.LeftShot(vec);
        }

    }

    public void PhysicalAttack()
    {
        Debug.Log("ﾁｪｽﾄｫｫｫｫ!!");
        m_enemy.m_attack.Approach(m_enemy._charaPara._leftAttack);
    }

    public void ResetTime()
    {
        m_time = m_cooltime * 60;
    }

}
