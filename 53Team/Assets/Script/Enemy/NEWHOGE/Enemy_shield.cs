using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy_shield : BoneCollide {

    public int m_shieldHp = 3;

    private void Start()
    {
        OnDamage.Subscribe(x => 
        {
            m_shieldHp--;
            if(m_shieldHp <= 0)
            {
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1);
            }
        }).AddTo(this);
    }
}
