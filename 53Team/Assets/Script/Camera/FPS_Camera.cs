using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Camera : MonoBehaviour {

    private GameObject targetObj;
    Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        targetObj = GameObject.Find("Player_Sample");
    }

    // Update is called once per frame
    void Update()
    {
        // ターゲットの位置を取得
        targetPos = targetObj.transform.position;
        // マウスを押している間
        if (Input.GetMouseButton(1))
        {
            // マウスの移動量
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");

            // targetの位置のY軸を中心に、回転(公転)する
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            // カメラの垂直移動（角度制限なし）
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 200f);
        }
    }
}
