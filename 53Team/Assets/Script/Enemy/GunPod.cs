using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPod : weaponFire {

    [Space(10)]
    public int m_magazine;
    public float m_rate;

    public override void fire()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        for (int i = 0; i < m_magazine; i++)
        {
            base.fire();
            yield return new WaitForSeconds(m_rate);
        }
    }
}
