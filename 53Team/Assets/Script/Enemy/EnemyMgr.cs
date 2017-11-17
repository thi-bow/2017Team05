using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class EnemyMgr : MonoBehaviour {

    public EnemyPopPoint _popBoosPoint;
    public EnemyPopPoint[] _popPointList;
    public EnemyDataLink[] _squads;

    public bool IsBossDead()
    {
        return true;
    }

    public void PopBossEnemy()
    {
        _popBoosPoint.PopEnemy();
    }
}
