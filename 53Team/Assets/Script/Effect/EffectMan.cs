using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EffectMan : MonoBehaviour {

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
    }

    public static EffectMan Instance
    {
        get { return m_instance; }
        private set { m_instance = value; }
    }
}
