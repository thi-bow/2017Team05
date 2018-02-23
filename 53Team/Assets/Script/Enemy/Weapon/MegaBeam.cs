using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBeam : weaponFire
{
    [Space(10)]
    public Transform m_target;
    public Transform m_horizontalTurret;
    public float m_horizontalAxis;
    public AnimationCurve m_curve;

    public ParticleSystem m_chage_particle;

	// Use this for initialization
	void Start () {
        if (m_target != null)
        {
            m_target = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    public override GameObject fire()
    {
        Vector3 dir = m_target.position - Gun_End.position;
        dir.y = m_target.position.y;


        return null;
    }


}
