using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PargeApproach : MonoBehaviour {
    // 近接攻撃中か
    [SerializeField] private bool isApproach = false;
    // 攻撃力
    [SerializeField] private int hitAtk = 1500;
    // 攻撃時間
    [SerializeField] private float atkTime = 1.0f;
    // 範囲の広さ
    [SerializeField] private float area = 1.0f;

    // 近接攻撃の距離
    [SerializeField] private float distance = 1.0f;

    Action pargeAction = null;

    RaycastHit hit;

    // Use this for initialization
    void Start () {
        this.GetComponent<BoxCollider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void PargeAttack(int atk = 1500, Action action = null)
    {
        isApproach = true;
        GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(ApproachRun(atk));
        if (action != null)
        {
            pargeAction = action;
        }
    }

    IEnumerator ApproachRun(int atk = 1500)
    {

        if (isApproach)
        {
            Vector3 crePos = new Vector3(transform.position.x, transform.position.y + 0.5f, Vector3.forward.z + 1.0f);

            Debug.Log("パージ攻撃");
            yield return new WaitForSeconds(atkTime);
            Debug.Log("終了");
            GetComponent<BoxCollider>().enabled = false;

            isApproach = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag != this.gameObject.tag)
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.GetComponent<BoneCollide>().Damage(hitAtk, Weapon.Attack_State.approach);
        }
    }

    public int GetAtk
    {
        get { return hitAtk; }
    }

    public float GetDistance
    {
        get { return distance; }
    }

    private void OnDrawGizmos()
    {

    }
}
