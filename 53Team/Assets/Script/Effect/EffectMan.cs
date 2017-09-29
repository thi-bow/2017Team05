using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMan : MonoBehaviour {

    private static EffectMan m_instance = null;

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
    }

    public static EffectMan Instance
    {
        get { return m_instance; }
        private set { m_instance = value; }
    }

}
