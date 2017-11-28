using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyMgr : MonoBehaviour {

    public EnemyPopPoint _popBoosPoint;
    public EnemyPopPoint[] _popPointList;
    public EnemyDataLink[] _squads;

    private static EnemyMgr _i;

    public static EnemyMgr i
    {
        get { return _i; }
    }

    private void Awake()
    {
        if(_i == null)
        {
            _i = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public bool IsBossDead()
    {
        return true;
    }

    public void PopBossEnemy()
    {
        _popBoosPoint.PopEnemy();
    }


    // 範囲内の雑魚エネミーの状態を警戒状態にする
    public List<Enemy_Standard> GetWarningEnemys(Vector3 aPosition, float aRadius = 10.0f)
    {
        List<Enemy_Standard> list = new List<Enemy_Standard>();

        var p = GameObject.FindGameObjectWithTag("Player");

        var cols = Physics.OverlapSphere(aPosition, aRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            var e = cols[i].gameObject.GetComponent<Enemy_Standard>();
            if(e)
            {
                e.m_lastPosition = p.transform.position;
                e.ChangeState(standard_State.warning);
            }
        }

        return list;
    }
}
