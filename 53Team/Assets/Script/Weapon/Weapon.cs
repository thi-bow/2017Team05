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

    public enum Weapon_State
    {
        Gun,
        Laser,
        Shot,
        NULL
    }

    public Attack_State state;

    public Weapon_State state_W;

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

    // 着弾エフェクト
    public GameObject _hitEffe = null;
    // ビームエフェクト
    public GameObject _beamEffe = null;
    GameObject beamClone;
    // ビームエフェクト
    //public GameObject _flashEffe = null;
    //GameObject flashClone;

    //public GameObject nozzle;

    // 射撃時間
    private float ShotTime;

    int mask = 1 << 8 | 1 << 10;

    public Vector3 effPos;

    // Use this for initialization
    void Start () {
        //player = GameObject.Find("Player_Sample");

        _WeaponChange = this.gameObject.GetComponent<WeaponChange>();
        //nowWeapon = _WeaponChange.GetWeapon();

        CameraCheck();

        bullets = maxBullets;
        // スクリーンの中心
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        //nozzle = GameObject.Find("nozzle");
    }
	
	// Update is called once per frame
	void Update () {
        Reload();
        if (beamClone != null)
        {
            //beamClone.transform.position += tpsCamera.transform.forward  * distance * (Time.deltaTime * 3.0f);
            beamClone.transform.position += effPos * distance * (Time.deltaTime * 3.0f);

            Destroy(beamClone, 0.3f);
        }
    }

    public void Reload()
    {
        if (isReload == true)
        {
            reloadTime += Time.deltaTime;
            // リロード完了までの時間処理
            if (reloadTime > reloadFinishTime)
            {

                bullets = maxBullets;
                reloadTime = 0;
                isReload = false;
                SoundManger.Instance.PlaySE(11);
            }
        }
    }

    // 射撃
    public void Shooting(Camera tpsCamera)
    {

        if (tpsCamera != null)
        {
            this.tpsCamera = tpsCamera;
        }
        if (bullets > 0)
        {
            ShotTime += Time.deltaTime;
            //if (_flashEffe != null)
            //{
            //    flashClone = GameObject.Instantiate(_flashEffe, nozzle.transform.position, this.transform.rotation);
            //    Destroy(flashClone, 0.2f);
            //}
            if (ShotTime > 60.0f / minuteShot)
            {
                SoundManger.Instance.PlaySE(5);
                
                StartCoroutine(ShootingInterval());
                
            }
        }
        else
        {
            isReload = true;
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
        if (state_W == Weapon_State.Gun)
        {
            if (_beamEffe != null)
            {
                beamClone = GameObject.Instantiate(_beamEffe, this.transform.position, this.transform.rotation);
            }

            Vector3 shotPos = tpsCamera.ScreenToWorldPoint(center);
            Ray ray;

            ray = new Ray(shotPos, tpsCamera.transform.forward * distance);
            //Debug.DrawRay(ray.origin, ray.direction * distance,Color.blue,2.0f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, mask))
            {
                if (hit.collider.gameObject.GetComponent<BoneCollide>() != null && hit.collider.tag != "Player")
                {
                    hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Attack_State.shooting);
                }
                if (_hitEffe != null && hit.collider.tag != "Player")
                {
                    var hibana = Instantiate(_hitEffe);
                    hibana.transform.position = hit.point;

                    Destroy(hibana, 0.5f);
                }
                effPos = (hit.point - tpsCamera.transform.position).normalized;
            }
            else
            {
                effPos = tpsCamera.transform.forward;
            }
            ShotTime = 0;

        }
        if (state_W == Weapon_State.Shot)
        {

            Vector3[] vectores = new Vector3[6];

            Vector3 shotPos = tpsCamera.ScreenToWorldPoint(center);

            for (int i = 0; i < vectores.Length; i++)
            {
                Ray ray;
                ray = new Ray(shotPos, tpsCamera.transform.forward * distance);

                vectores[i] = new Vector3(Random.Range(0.0f, 3.0f), Random.Range(-3.0f, 3.0f), Random.Range(0.0f, 3.0f));

                Debug.DrawRay(ray.origin, ray.direction * 3.0f, Color.red, 2.0f);

                Debug.DrawRay(ray.origin, (ray.direction * 10 + vectores[i]) * 3.0f, Color.red, 2.0f);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10, mask))
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.gameObject.GetComponent<BoneCollide>() != null && hit.collider.tag != "Player")
                    {
                        hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Attack_State.shooting);
                    }
                }

            }
            ShotTime = 0;
        }
    }

    IEnumerator ShootingInterval(Ray shotRay)
    {
        yield return new WaitForSeconds(shotspeed);
        Ray ray;

        if (_beamEffe != null)
        {
            beamClone = GameObject.Instantiate(_beamEffe, this.transform.position, this.transform.rotation);
        }

        ray = shotRay;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance,mask))
        {
            if (hit.collider.gameObject.GetComponent<BoneCollide>() != null)
            {
                hit.collider.gameObject.GetComponent<BoneCollide>().Damage(atk, Attack_State.shooting);
            }
            if (_hitEffe != null)
            {
                var hibana = Instantiate(_hitEffe);
                hibana.transform.position = hit.point;
                Destroy(hibana, 0.5f);
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

    }

    // 攻撃力の取得
    public int GetAtk
    {
        get { return atk; }
    }
}
