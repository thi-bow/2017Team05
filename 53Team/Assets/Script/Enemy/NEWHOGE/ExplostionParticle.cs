using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplostionParticle : MonoBehaviour{

    public ParticleSystem[] m_explostionParticles;

    public void Play()
    {
        for (int i = 0; i < m_explostionParticles.Length; i++)
        {
            if(m_explostionParticles[i])
                m_explostionParticles[i].Play();
        }
    }
}
