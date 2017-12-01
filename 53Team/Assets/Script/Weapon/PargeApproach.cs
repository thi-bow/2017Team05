using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PargeApproach : MonoBehaviour {
    // 近接攻撃中か
    [SerializeField] private bool isApproach = false;
    // 攻撃力
    [SerializeField] private int hitAtk = 2000;
    // 攻撃時間
    [SerializeField] private float atkTime = 1.0f;
    // 範囲の広さ
    [SerializeField] private float area = 1.0f;

    // 近接攻撃の距離
    [SerializeField] private float distance = 1.0f;

    RaycastHit hit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void PargeAttack(int atk)
    {
        StartCoroutine(ApproachRun(atk));
    }

    IEnumerator ApproachRun(int atk)
    {
        Debug.Log("パージ攻撃");

        Vector3 crePos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.BoxCast(crePos, Vector3.one * area, transform.forward, out hit, transform.rotation,distance))
        {
            if (hit.collider.gameObject.tag != this.gameObject.tag)
            {
                Debug.Log(hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Weapon.Attack_State.approach);
            }
        }

        yield return new WaitForSeconds(atkTime);
        Debug.Log("終了");

        isApproach = false;
    }
}
