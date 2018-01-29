using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyMgr : MonoBehaviour {

    public GameObject m_Boss;
    public EnemyPopPoint[] m_popPointList;
    public EnemyDataLink[] m_squads;

    private bool m_isBossDead = false;


    private static EnemyMgr m_i;
    public static EnemyMgr i
    {
        get { return m_i; }
    }

    private void Awake()
    {
        if(m_i == null)
        {
            m_i = this;
        }
        else
        {
            Destroy(this);
        }

        if (m_Boss)
        {
            m_Boss.SetActive(false);
        }
    }

    public void BossDead()
    {
        ResultScore.KillCount++;
        m_isBossDead = true;
    }

    public bool IsBossDead()
    {
        return m_isBossDead;
    }

    public void PopBossEnemy()
    {
        m_Boss.gameObject.SetActive(true);
    }

    public void OnDeadEnemy(int aGroup = 0)
    {
        ResultScore.KillCount++;

        if(aGroup != 0)
        {
            DeadSquad(aGroup);
        }
    }

    public void DeadSquad(int aGroup)
    {
        for (int i = 0; i < m_popPointList.Length; i++)
        {
            if(m_popPointList[i].m_group.group == aGroup)
            {
                m_popPointList[i].CheckSquad();
                break;
            }
        }
    }


    // 範囲内の雑魚エネミーの状態を警戒状態にする
    public List<Enemy_Standard> GetWarningEnemys(Vector3 aPosition, float aRadius = 10.0f)
    {
        List<Enemy_Standard> list = new List<Enemy_Standard>();

        var p = GameObject.FindGameObjectWithTag("Player");

        // StartCoroutine(ShowWarningArea(aPosition, aRadius));
        var cols = Physics.OverlapSphere(aPosition, aRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            var e = cols[i].gameObject.GetComponent<Enemy_Standard>();
            if(e)
            {
                e.m_lastPosition = p.transform.position;

                if(e.IsCurrentState(standard_State.move))
                    e.ChangeState(standard_State.warning);
            }
        }

        return list;
    }

    //private Vector3 pos;
    //private float r;
    //IEnumerator ShowWarningArea(Vector3 aPos, float aRadius)
    //{
    //    pos = aPos;
    //    r = aRadius;

    //    yield return new WaitForSeconds(3);

    //    pos = new Vector3(0, 0, 0);
    //    r = 0f;
    //}

    //private void OnDrawGizmos()
    //{
    //    if(pos != Vector3.zero && r != 0)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(pos, r);
    //    }
    //}
}
