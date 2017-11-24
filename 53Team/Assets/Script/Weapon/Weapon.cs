using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public enum Attack_State
    {
        shooting,
        approach,
        NULL
    }

    public Attack_State state;

    private GameObject nowWeapon;

    private WeaponChange _WeaponChange;
    [SerializeField] private Camera tpsCamera;

    [Header("残弾数")]
    // 残弾数
    public int bullets;
    [Header("最高装填数")]
    // 最高装填数
    public int maxBullets;
    [Header("1分間に何発撃てるか")]
    // 分間射撃数
    public float minuteShot;
    [Header("攻撃力")]
    // 威力
    public int atk;
    [Header("距離")]
    // 距離
    public float distance;

    // 弾速
    public float shotspeed = 0.01f;

    // リロードするか
    [SerializeField]
    private bool isReload = false;
    // リロード完了までの経過時間
    private float reloadTime = 0f;

    [Header("リロード時間")]
    // リロード完了までにかかる時間の設定
    [SerializeField]
    private float reloadFinishTime;

    // スクリーン中央取得用
    private Vector3 center;

    // 射撃時間
    private float ShotTime;

    int mask = 1 << 8;

    // Use this for initialization
    void Start () {
        //player = GameObject.Find("Player_Sample");

        _WeaponChange = this.gameObject.GetComponent<WeaponChange>();
        //nowWeapon = _WeaponChange.GetWeapon();

        CameraCheck();

        bullets = maxBullets;
        // スクリーンの中心
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    _WeaponChange.Change();
        //}
    }

    public void Reload()
    {
        if (bullets <= 0)
        {
            isReload = true;
        }

        if (isReload == true)
        {
            reloadTime += Time.deltaTime;
            // リロード完了までの時間処理
            if (reloadTime > reloadFinishTime)
            {
                Debug.Log("リロード");
                bullets = maxBullets;
                reloadTime = 0;
                isReload = false;
            }
        }
    }

    // 射撃
    public void Shooting(Camera tpsCamera)
    {
        Reload();
        if (tpsCamera != null)
        {
            this.tpsCamera = tpsCamera;
        }
        if (bullets >= 0)
        {
            ShotTime += Time.deltaTime;
            if (ShotTime >= 60.0f / minuteShot)
            {
                StartCoroutine(ShootingInterval());
                
            }
        }
    }

    // 射撃(Ray)
    public void Shooting(Ray shotRay) {
        if (bullets >= 0)
        {
            ShotTime += Time.deltaTime;
            if (ShotTime >= 60.0f / minuteShot)
            {
                bullets--;
                StartCoroutine(ShootingInterval(shotRay));
            }
        }
    }

    IEnumerator ShootingInterval()
    {
        yield return new WaitForSeconds(shotspeed);
        bullets--;
        Vector3 shotPos = tpsCamera.ScreenToWorldPoint(center);
        Ray ray;

        ray = new Ray(shotPos, tpsCamera.transform.forward * distance);

        //GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        //b.transform.position = tpsCamera.transform.forward * 10;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance,mask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.GetComponent<BoneCollide>() != null)
            {
                print("Hit");
                hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Attack_State.shooting);
            }
        }
        ShotTime = 0;
    }

    IEnumerator ShootingInterval(Ray shotRay)
    {
        yield return new WaitForSeconds(shotspeed);
        Ray ray;

        ray = shotRay;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance,mask))
        {
            if (hit.collider.gameObject.GetComponent<BoneCollide>() != null)
            {
                hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Attack_State.shooting);
            }
        }
        ShotTime = 0;
    }

        public void Aim()
    {
    }

    public void SetWeapon(GameObject obj)
    {
        nowWeapon = obj;
    }

    public GameObject GetWeapon()
    {
        return nowWeapon;
    }

    // 現在のカメラ
    public void CameraCheck()
    {
        /*if (fpsCamera.activeInHierarchy)
        {
            transform.parent = homing.transform;

        }
        if (tpsCamera.activeInHierarchy)
        {
            transform.parent = player.transform;
            //reticle.SetActive(false);
        }*/
    }

    // 攻撃力の取得
    public int GetAtk
    {
        get { return atk; }
    }

    public void CameraMove(Vector3 move)
    {
        tpsCamera.transform.position += move;
    }

    public void StateChange(Attack_State state)
    {

    }
}
