using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachAttack : MonoBehaviour
{

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

    public GameObject _AppEff = null;
    GameObject AppClone;

    public Camera tpsCamPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 近接攻撃
    public void Approach(int atk)
    {
        // コンボカウント
        comboCount++;
        if ((!isApproach || isCombo == true))
        {
            isApproach = true;
            //SoundManger.Instance.PlaySE(0);
            StartCoroutine(ApproachRun(atk));
        }
        if (comboCount > 1)
        {
            isCombo = true;
        }
        if (comboCount >= maxCombo)
        {
            isCombo = false;
        }
    }

    IEnumerator ApproachRun(int atk)
    {
        if (maxCombo >= comboCount)
        {
            Debug.Log("近接" + comboCount + "発目");

            Vector3 crePos = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
            if (_AppEff != null)
            {
                int mask = 1 << 8;
                if (Physics.SphereCast(crePos, 1.0f, tpsCamPos.transform.forward, out hit, distance, mask))
                {
                    if (hit.collider.gameObject.tag != this.gameObject.tag)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Weapon.Attack_State.approach);
                        var app = Instantiate(_AppEff);
                        app.transform.position = hit.point;

                        Destroy(app, 1.0f);
                    }
                    else
                    {
                        AppClone = GameObject.Instantiate(_AppEff, crePos + tpsCamPos.transform.forward * 1.5f, this.transform.rotation);
                        Destroy(AppClone, 1.0f);
                    }
                }
                else
                {
                    AppClone = GameObject.Instantiate(_AppEff, crePos + tpsCamPos.transform.forward * 1.5f, this.transform.rotation);
                    Destroy(AppClone, 1.0f);
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

    public int getAtk
    {
        get { return hitAtk; }
    }

    public float GetDistance
    {
        get { return distance; }
    }
}
