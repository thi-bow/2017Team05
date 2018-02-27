using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPod : weaponFire {

    [Space(10)]
    public int m_magazine;
    public float m_rate;
    public float m_waitTime;

    protected override void Awake()
    {
        base.Awake();
        Preroad();
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
            base.fire();
            yield return new WaitForSeconds(m_rate);
        }
    }
}
