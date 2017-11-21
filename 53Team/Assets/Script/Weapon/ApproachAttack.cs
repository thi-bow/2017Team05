using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachAttack : MonoBehaviour {

    // 近接攻撃中か
    [SerializeField] private bool isApproach = false;

    // 攻撃力
    [SerializeField] private int hitAtk = 10;

    // 攻撃時間
    [SerializeField] private float atkTime = 1.0f;

    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Approach(hitAtk);
        }
    }

    // 近接攻撃
    public void Approach(int atk)
    {
        if (!isApproach)
        {
            isApproach = true;
            StartCoroutine(ApproachRun(atk));
        }
    }

    IEnumerator ApproachRun(int atk)
    {
        Debug.Log("近接");

        Vector3 crePos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.SphereCast(crePos, 0.5f, transform.forward, out hit, 1.0f))
        {
            hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Weapon.Attack_State.approach);
        }

        yield return new WaitForSeconds(atkTime);

        Debug.Log("終了");

        isApproach = false;
    }


}
