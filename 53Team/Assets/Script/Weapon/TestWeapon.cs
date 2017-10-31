using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWeapon : MonoBehaviour, Enemy.IEnemy {

    // TPSカメラ
    [SerializeField]
    private GameObject tpsCamera;
    // FPSカメラ
    [SerializeField]
    private GameObject fpsCamera;

    // 残弾数
    public int bullets;
    // 最高装填数
    public int maxBullets;
    // 分間銃撃数
    public float m;
    // 威力
    public int atk;
    // 距離
    public float distance;
    // トータル威力
    public int total_atk;

    // リロードするか
    [SerializeField]
    private bool isReload = false;
    // リロード完了までの経過時間
    private float reloadTime = 0f;
    // リロード完了までにかかる時間の設定
    [SerializeField]
    private float reloadFinishTime;

    // スクリーン中央取得用
    private Vector3 center;

    // 照準のポジション
    public GameObject aimPos;
    public GameObject setPos;

    public GameObject nozzle;

    private float ShotTime;

    private bool isAim = false;

    private Vector3 hitPoint;
    private bool isRayHit = false;

    private LineRenderer line;

    private float rayDis = 100.0f;
    private float shotDis = 100.0f;

    public GameObject bullet;
    public float power;

    public GameObject reticle;

    // Use this for initialization
    void Start () {

        bullets = maxBullets;
        // スクリーンの中心
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        line = GetComponent<LineRenderer>();
        line.SetWidth(0.1f,0.1f);

        reticle.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        // 武器照準
        Aim();
    }

    // 射撃
    public void Shot() {
        if(fpsCamera.activeInHierarchy)
        {
            if (Input.GetMouseButton(0) && bullets > 0)
            {

                ShotTime += Time.deltaTime;
                if (ShotTime >= 60.0f / m)
                {
                    GameObject bulletInstance = GameObject.Instantiate(bullet, nozzle.transform.position, Quaternion.identity) as GameObject;
                    Vector3 force;
                    force = fpsCamera.transform.forward * power;

                    bullets--;
                    bulletInstance.GetComponent<Rigidbody>().AddForce(force,ForceMode.Acceleration);
                    bulletInstance.transform.position = nozzle.transform.position;
                    Destroy(bulletInstance, 1);
                    ShotTime = 0;
                }
            }

            /*if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) || Input.GetKeyDown(KeyCode.E) && bullets > 0)
            {
                var bulletInstance = GameObject.Instantiate(bullet, nozzle.transform.position, nozzle.transform.rotation);
                Ray ray;
                ShotTime += Time.deltaTime;
                if (ShotTime >= 60.0f / m)
                {
                    bullets--;
                    bulletInstance.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * power);
                    //bulletInstance.GetComponent.<Rigidbody>().AddForce(bulletInstance.transform.forward * bulletPower);
                    ShotTime = 0;
                }



                ray = fpsCamera.GetComponent<Camera>().ScreenPointToRay(center);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    if (hit.collider.name == "MotionGun")
                    {
                        hit.collider.GetComponent<Enemy.Enemy_Turret>().Damage(atk);
                    }
                }
            }
            else
            {
                ShotTime = 0;
            }
            */
        }
        if (bullets > 0)
        {

            ShotTime += Time.deltaTime;
            if (ShotTime >= 60.0f / m)
            {
                bullets--;
                Vector3 shotPos = tpsCamera.GetComponent<Camera>().ScreenToWorldPoint(center);
                Ray ray;
                ray = new Ray(shotPos, tpsCamera.transform.forward * distance);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    Debug.Log(hit.collider.gameObject);

                    if (hit.collider.tag == "Enemy")
                    {

                    }
                }

                /*GameObject bulletInstance = GameObject.Instantiate(bullet, shotPos, tpsCamera.transform.rotation) as GameObject;

                Vector3 force;

                force = tpsCamera.transform.forward * power;

                bullets--;
                bulletInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
                bulletInstance.transform.position = shotPos;
                ShotTime = 0;*/
            }
        }
        else
        {
            ShotTime = 0;
        }
    }

    // リロード処理
    public void Reload() {
        // リロードボタンを押したら
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isReload = true;
        }

        if (bullets <= maxBullets && isReload == true) {
            reloadTime += Time.deltaTime;
            // リロード完了までの時間処理
            if (reloadTime > reloadFinishTime) {
                Debug.Log("リロード");
                bullets = maxBullets;
                reloadTime = 0;
                isReload = false;
            }
        }
    }
    // エイム
    public void Aim() {
        if (Input.GetKey(KeyCode.Q)) {
            isAim = true;
        } else {
            isAim = false;
            
        }
        if (fpsCamera.activeInHierarchy)
        {
            if (isAim)
            {
                reticle.SetActive(true);
                transform.position = Vector3.Lerp(transform.position, aimPos.transform.position, Time.deltaTime * 5.0f);
            }
            else
            {
                reticle.SetActive(false);
                transform.position = Vector3.Lerp(transform.position, setPos.transform.position, Time.deltaTime * 5.0f);
            }
        }
        if (tpsCamera.activeInHierarchy)
        {
            if (isAim)
            {
                reticle.SetActive(true);

                /*line.SetVertexCount(2);
                line.SetPosition(0, nozzle.transform.position);

                Ray ray;
                ray = new Ray(nozzle.transform.position, tpsCamera.transform.forward);

                RaycastHit hit;
                isRayHit = false;
                rayDis = distance;

                if (Physics.Raycast(ray, out hit, rayDis))
                {
                    isRayHit = true;
                    hitPoint = hit.point;
                    line.SetPosition(1, hit.point);
                }*/
            }
            else
            {
                reticle.SetActive(false);
                line.SetVertexCount(0);
            }
        }
    }

    // 攻撃力の取得
    public float GetAtk
    {
        get { return atk; }
    }

    void LineOn()
    {
        line.enabled = true;
    }

     void LineOff()
    {
        line.enabled = false;
    }

    public void Damage(int damage)
    {
        Debug.Log("damage");
    }

    public void DestroyChild(Transform parent_trans)
    {
        for (int i = 0; i < parent_trans.childCount; i++)
        {
            GameObject.Destroy(parent_trans.GetChild(i).gameObject);
        }
    }
}
