using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;


[Serializable]
public class Enemy_Boss_Bhavior{

    public enum rule
    {
        sequence = 0,
        priority = 1,
        randm = 2,
    }

    public enum hierarchy
    {
        check = 0,
        command = 1,
        skill = 2,
        action = 3
    }

    public Dictionary<hierarchy, rule> m_tree = new Dictionary<hierarchy, rule>();

    public Enemy_Boss_Bhavior()
    {
    }
    
    public void Initialize()
    {
        m_tree.Add(hierarchy.check, rule.sequence);
        m_tree.Add(hierarchy.command, rule.priority);
        m_tree.Add(hierarchy.skill, rule.randm);
        m_tree.Add(hierarchy.action, rule.sequence);
    }

    public void Update()
    {

    }

    public void End()
    {

    }
}
