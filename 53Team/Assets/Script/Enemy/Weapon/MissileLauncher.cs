using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MissileLauncher : weaponFire{

    [Space(10)]
    public Transform m_target;
    public int m_magazine;
    public float m_rate;
    public float m_waitTime;

    private void Start()
    {
        if (m_target != null)
        {
            m_target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public override GameObject fire()
    {
        StartCoroutine(Wait());

        return null;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(m_waitTime);

        for (int i = 0; i < m_magazine; i++)
        {
            var missile = base.fire().GetComponent<Homing_bullet>();
            Observable.Timer(System.TimeSpan.FromSeconds(0.3f)).Subscribe(_ => 
            {
                missile.m_target = m_target;
            }).AddTo(this);
            yield return new WaitForSeconds(m_rate);
        }
    }
}

