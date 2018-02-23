using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponFire : MonoBehaviour {
	public Shell Shell;
	public Transform Gun_End;

	public float shellSpeed=500;
	public float randomDir=20;

	public ParticleSystem m_smokeBarrel;    //Particle effect shot  
	public AudioSource m_AudioSource;  	//Sound effect shot 
	public AudioClip soundFire;

    private ShellPool m_shellPool;

    private void Awake()
    {
        m_shellPool = new ShellPool(Shell);
    }

    public virtual GameObject fire() //shot
	{
		// var gameOb = Instantiate(Shell,  Gun_End.transform.position,Gun_End.transform.rotation);
        var gameOb = m_shellPool.Rent();
        gameOb.transform.SetPositionAndRotation(Gun_End.transform.position, Gun_End.transform.rotation);
        gameOb.m_pool = m_shellPool;

        Vector3 dir = new Vector3(Random.Range(-randomDir, randomDir), Random.Range(-randomDir, randomDir), Random.Range(-randomDir,randomDir)) ;
		dir+=Gun_End.forward*shellSpeed;
		gameOb.GetComponent<Rigidbody>().AddForce(dir);

		if(m_smokeBarrel) m_smokeBarrel.Play();
		if (m_AudioSource) {
			m_AudioSource.clip =  soundFire;
			m_AudioSource.Play ();
		}

        return gameOb.gameObject;
	}

    private void OnDestroy()
    {
        m_shellPool.Clear();
    }
}
 