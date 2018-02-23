using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PargeShot : MonoBehaviour {

    public enum Attack_State_Parge
    {
        shooting,
        approach,
        NULL
    }

    public Attack_State_Parge state;

    [SerializeField] private Camera tpsCamera;

    [Header("残弾数")]
    // 残弾数
    public int bullets = 10;
    [Header("最高装填数")]
    // 最高装填数
    public int maxBullets = 0;
    [Header("1分間に何発撃てるか")]
    // 分間射撃数
    public float minuteShot = 300.0f;
    [Header("攻撃力")]
    // 威力
    public int atk = 50;
    [Header("距離")]
    // 距離
    public float distance = 100;

    // スクリーン中央取得用
    private Vector3 center;

    // 射撃時間
    private float ShotTime;

    // 弾速
    public float shotspeed = 0.01f;

    // フルパージされたか
    [SerializeField] private bool isParge = false;

    int mask = 1 << 8;

    // パージエフェクト
    public GameObject pargeEff = null;
    GameObject pargeClone;

    Action pargeAction = null;

    public Vector3 pargePos = new Vector3(0.0f, 1.0f, 0.0f);

    // Use this for initialization
    void Start () {

        tpsCamera = Camera.main;

        maxBullets = bullets;
        // スクリーンの中心
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (isParge)
        {
            StartCoroutine(ShootingInterval());
        }
        if (bullets <= 0)
        {
            isParge = false;
            pargeAction();
            bullets = maxBullets;
        }
    }

    public void PargeAttack(Camera tpsCamera, Action action = null)
    {
        if (tpsCamera != null)
        {
            this.tpsCamera = tpsCamera;
        }
        isParge = true;
        if (pargeEff != null)
        {
            SoundManger.Instance.PlaySE(18);

            transform.rotation = Quaternion.LookRotation(tpsCamera.transform.forward);
            pargeClone = GameObject.Instantiate(pargeEff, transform.position + pargePos, transform.rotation);
            Destroy(pargeClone, 2.5f);
        }

        if (action != null)
        {
            pargeAction = action;
        }
    }

    IEnumerator ShootingInterval()
    {
        yield return new WaitForSeconds(shotspeed);

        ShotTime += Time.deltaTime;
        if (ShotTime >= 60.0f / minuteShot)
        {
            bullets--;
            Vector3 shotPos = tpsCamera.GetComponent<Camera>().ScreenToWorldPoint(center);
            Ray ray;

            ray = new Ray(shotPos, tpsCamera.transform.forward * distance);
            Debug.Log("PargeShot!");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, mask))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.GetComponent<BoneCollide>() != null)
                {
                    print("Hit");
                    hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Weapon.Attack_State.shooting);
                }
            }
            ShotTime = 0;
        }
    }
}
