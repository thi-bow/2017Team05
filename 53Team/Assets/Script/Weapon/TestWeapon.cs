using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWeapon : MonoBehaviour {

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
    public float atk;
    // 距離
    public float distance;

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

    public float ShotTime;

    private bool isAim = false;

    public GameObject mText;
    public GameObject bText;

    // Use this for initialization
    void Start () {
        bullets = maxBullets;
        // スクリーンの中心
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
	
	// Update is called once per frame
	void Update () {

        // 武器照準
        Aim();

        bText.GetComponent<Text>().text = bullets.ToString();
        mText.GetComponent<Text>().text = maxBullets.ToString();
    }

    // 射撃
    public void Shot() {
        if(fpsCamera.activeInHierarchy)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) || Input.GetKeyDown(KeyCode.E) && bullets > 0)
            {
                Ray ray;
                ShotTime += Time.deltaTime;
                if (ShotTime >= 60.0f / m)
                {
                    bullets--;
                    ShotTime = 0;
                }

                ray = fpsCamera.GetComponent<Camera>().ScreenPointToRay(center);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    print("TestWeapon : " + hit.transform.name);
                }
            }
            else
            {
                ShotTime = 0;
            }
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
        if (isAim) {
            transform.position = Vector3.Lerp(transform.position, aimPos.transform.position, Time.deltaTime * 5.0f);
        } else {
            transform.position = Vector3.Lerp(transform.position, setPos.transform.position, Time.deltaTime * 5.0f);
        }
    }

    // 攻撃力の取得
    public float GetAtk()
    {
        return atk;
    }
}
