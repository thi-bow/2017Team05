using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HogeColMan : CharaBase {

    [Header("各部位の当たり判定")]
    public BoneCollide[] m_boneCollides;

    [Header("デバッグ用パーツ")]
    public Armor m_armor;

    protected override void Start()
    {
        if (m_armor != null) { PartsAdd(Parts.RightArm, m_armor); }


        for (int i = 0; i < m_boneCollides.Length; i++)
        {
            int n = i;
            m_boneCollides[n].OnDamage.Subscribe(dmg => 
            {
                Parts parts = m_boneCollides[n].m_parts;
                Debug.LogFormat("Hit!!!!!!  Parts.{0} {1}damage", parts.ToString(), dmg);
                PartsDamage(dmg, parts, () => Debug.Log(parts + "パージ!!"));
            });
        }
    }
}
