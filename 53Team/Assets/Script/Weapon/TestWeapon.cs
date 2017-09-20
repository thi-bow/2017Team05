using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWeapon : MonoBehaviour {

    // FPSカメラ
    [SerializeField]
    private GameObject fpsCamera;

    // 残弾数
    public int bullets;
    // 最高装填数
    public int maxBullets;
    // 分間銃撃数
    public float m;

    // リロード
    [SerializeField]
    private bool isReload = false;
    [SerializeField]
    private float reloadTime = 0f;
    [SerializeField]
    private float reloadFinishTime;

    // 照準
    private Vector3 aimPosition;

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
        fpsCamera = GameObject.Find("FPS_Camera");
    }
	
	// Update is called once per frame
	void Update () {
        // 銃撃
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && bullets > 0) {
            Shot();
        } else {
            ShotTime = 0;
        }

        // リロード処理
        if (Input.GetKeyDown(KeyCode.Z)) {
            isReload = true;
        }
        if (isReload) {
            Reload();
        }
        // 武器照準
        Aim();

        bText.GetComponent<Text>().text = bullets.ToString();
        mText.GetComponent<Text>().text = maxBullets.ToString();
    }

    public void Shot() {
        Ray ray;
        ShotTime += Time.deltaTime;
        if (ShotTime >= 60.0f / m) {
            bullets--;
            ShotTime = 0;
        }

        ray = fpsCamera.GetComponent<Camera>().ScreenPointToRay(center);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            print(hit.transform.name);
        }
    }

    public void Reload() {
        if (bullets < maxBullets && isReload == true) {
            reloadTime += Time.deltaTime;

            if (reloadTime > reloadFinishTime) {
                Debug.Log("リロード");
                bullets = maxBullets;
                reloadTime = 0;
                isReload = false;
            }
        }
    }

    public void Aim() {
        if (Input.GetKey(KeyCode.Q)) {
            isAim = true;
        } else {
            isAim = false;
            
        }
        if (isAim) {
            transform.position = Vector3.Lerp(transform.position, aimPos.transform.position, Time.deltaTime * 2.0f);
        } else {
            transform.position = Vector3.Lerp(transform.position, setPos.transform.position, Time.deltaTime * 2.0f);
        }
    }
}
