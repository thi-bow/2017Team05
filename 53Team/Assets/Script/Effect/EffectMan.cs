using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EffectMan : MonoBehaviour {


    [Header("弾プレハブ")]
    public NormalBullet m_nBullet;
    private NormalBulletPool m_nBulletPool;

    private static EffectMan m_instance = null;

    private void Awake()
    {
        if(m_instance != null)
        {
            Destroy(this);
            return;
        }

        m_instance = this;
    }

    private void Start()
    {
        m_nBulletPool = new NormalBulletPool(m_nBullet, transform);
    }

    public static EffectMan Instance
    {
        get { return m_instance; }
        private set { m_instance = value; }
    }


    public void NormalBullet(Transform aPos, Transform aTage, Action aOnHit)
    {
        var b = m_nBulletPool.Rent();
        b.Shot(aPos, aTage, aOnHit).Subscribe(_ => {
            m_nBulletPool.Return(b);
        });
    }
}
