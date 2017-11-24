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

    // 近接攻撃の距離
    [SerializeField] private float distance = 1.0f;
    // 最高コンボ数
    [SerializeField] private int maxCombo = 3;
    // コンボカウント
    [SerializeField] private int comboCount = 0;
    // コンボ中か
    [SerializeField] private bool isCombo = false;

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
        // コンボカウント
        comboCount++;
        if ((!isApproach || isCombo == true))
        {
            isApproach = true;
            StartCoroutine(ApproachRun(atk));
        }
        if (comboCount > 1)
        {
            isCombo = true;
        }
    }

    IEnumerator ApproachRun(int atk)
    {
        if (maxCombo >= comboCount)
        {
            Debug.Log("近接" + comboCount + "発目");

            
            Vector3 crePos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            if (Physics.SphereCast(crePos, 0.5f, transform.forward, out hit, distance))
            {

                if (hit.collider.gameObject.tag != this.gameObject.tag)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Weapon.Attack_State.approach);
                }
            }
        }
        else
        {
            comboCount = 0;
        }
        yield return new WaitForSeconds(atkTime);
        Debug.Log("終了");
        comboCount = 0;

        isApproach = false;
    }
}
