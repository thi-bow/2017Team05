using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;


[Serializable]
public class Enemy_Boss_Behavior{

    public enum rule
    {
        sequence = 0,
        priority = 1,
        randm = 2,
    }

    public class Node
    {
        public int      priority;   // 優先度
        public Action   action;     // 実行イベント

        public Node(int aPriority, Action aAction)
        {
            priority = aPriority;
            action = aAction;
        }
    }

    public class Hierarchy
    {
        public rule rule;
        public List<Node> contents;

        public Hierarchy(rule aRule, List<Node> aList)
        {
            rule = aRule;
            contents = aList;
        }
    }

    public List<Hierarchy> m_behaviorTree;

    private Enemy_Boss_State m_base;

    public Enemy_Boss_Behavior(Enemy_Boss_State aEnemy)
    {
        m_base = aEnemy;

    }

    public void Initialize()
    {
    }

    public void Update()
    {
    }

    public void End()
    {
    }

    public int GetHp()
    {
        return m_base._charaPara._hp;
    }
}
