using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HogeColMan : CharaBase {

    [Header("各部位の当たり判定")]
    public BoneCollide[] m_boneCollides;

    protected override void Start()
    {
        for (int i = 0; i < m_boneCollides.Length; i++)
        {
            int n = i;
            m_boneCollides[n].OnDamage.Subscribe(dmg => 
            {
                Debug.LogFormat("Hit!!!!!!  Parts.{0} {1}damage", m_boneCollides[n].m_parts.ToString(), dmg);
                PartsDamage(dmg, m_boneCollides[n].m_parts);
            });
        }
    }
}
