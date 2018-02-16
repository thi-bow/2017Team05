using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

//this script is for the  shell
public class Shell : MonoBehaviour, IObjectPool {
	public float lifeTime = 2.0f;
	public int shellDamage = 10;
	public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
    public ParticleSystem m_SmokeParticles;
    public AudioSource m_ExplosionAudio;

    public ShellPool m_pool;

    protected virtual void OnTriggerEnter(Collider col){

        // Play the particle system.
        if (m_ExplosionParticles) m_ExplosionParticles.Play();
		// Play the explosion sound effect.
		if(m_ExplosionAudio)m_ExplosionAudio.Play();
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody> ().velocity =Vector3.zero;
		GetComponent<Collider> ().enabled = false;
		GetComponentInChildren<Renderer> ().enabled = false;
        StartCoroutine(Dead(2)); 

 
 
	}

    protected virtual IEnumerator Init()
	{
		yield return new WaitForSeconds(0.1f);
		GetComponent<Collider> ().enabled = true;
        if (m_SmokeParticles) m_SmokeParticles.Play();
		yield return new WaitForSeconds(lifeTime - 0.1f);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Renderer>().enabled = false;
        StartCoroutine(Dead());

    }

    protected virtual IEnumerator Dead(float time = 0)
    {
        if (m_SmokeParticles)
        {
            m_SmokeParticles.Stop();
            yield return new WaitWhile(() => m_SmokeParticles.IsAlive());
        }

        yield return new WaitForSeconds(time);
        m_pool.Return(this);
    }

    public virtual void OnRent()
    {
        StartCoroutine(Init());
    }

    public virtual void OnReturn()
    {
    }

    public virtual void OnClear()
    {
    }
}

public class ShellPool : ObjectPool<Shell>
{
    private readonly Shell m_prefab;

    public ShellPool(Shell prefab)
    {
        m_prefab = prefab;
    }

    protected override Shell CreateInstance()
    {
        return GameObject.Instantiate(m_prefab);
    }

    protected override void OnBeforeRent(Shell instance)
    {
        base.OnBeforeRent(instance);
        instance.OnRent();
    }

    protected override void OnBeforeReturn(Shell instance)
    {
        instance.OnReturn();
        base.OnBeforeReturn(instance);
    }
}
