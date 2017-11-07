using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

     private Player _player = null;

    public Weapon _weapon;

    public GameObject hitObj;

    private GameObject RightArm;
    private GameObject LeftArm;

    private bool isAtk = false;

    private float atkTimer = 1.0f;

    private float timer;

    // Use this for initialization
    void Start () {
        _player = GameObject.Find("Player_Sample").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            // 射撃
            _weapon.Shooting();
        }
        // リロード
        _weapon.Reload();

        _weapon.CameraCheck();

        timer += Time.deltaTime;
        if (timer > atkTimer)
        {
            isAtk = false;
            timer = 0;
        }
    }

    public void ApproachAtk()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RightPunch();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LeftPunch();
        }
    }

    void LeftPunch()
    {
        // アニメーション

        // 左腕に変更
        GameObject hit = GameObject.Instantiate(hitObj, new Vector3(_player.transform.position.x, _player.transform.position.y + 1.0f, _player.transform.position.z + 1.0f), Quaternion.identity) as GameObject;
        Vector3 force;
        force = _player.transform.forward * 10;

        hit.GetComponent<Rigidbody>().AddForce(force);

        Destroy(hit, 1.5f);
    }

    void RightPunch()
    {
        // 右腕に変更
        GameObject hit = GameObject.Instantiate(hitObj, new Vector3(_player.transform.position.x, _player.transform.position.y + 1.0f, _player.transform.position.z + 1.0f), Quaternion.identity) as GameObject;
        Vector3 force;
        force = _player.transform.forward * 10;

        hit.GetComponent<Rigidbody>().AddForce(force);

        Destroy(hit, 1.5f);
    }
}
