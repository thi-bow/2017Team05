using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadAnimation : MonoBehaviour {

    private Rigidbody m_rd;

    public Animator m_anim;

	// Use this for initialization
	void Start () {
        m_rd = GetComponent<Rigidbody>();
	}


    public void Anima()
    {
        StartCoroutine(anim());
    }

    IEnumerator anim()
    {
        m_rd.useGravity = true;
        m_rd.isKinematic = false;
        // m_rd.velocity = transform.up * -1;
        m_anim.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        EnemyMgr.i.BossDead();
    }
}
