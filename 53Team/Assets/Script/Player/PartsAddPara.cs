using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartsAddPara : MonoBehaviour {

    #region 右腕のパーツ装着座標と回転
    public static Vector3[] PlayerRightArmPosition = 
    {
        new Vector3(0.18f, 0, 0.1f),
        new Vector3(0.18f, -0.1f, 0),
        new Vector3(0.18f,  0.1f, 0),
        new Vector3(0.18f, 0, -0.1f),
        new Vector3(-0.1f, 0, 0.1f),
        new Vector3(-0.1f, -0.1f, 0),
        new Vector3(-0.1f,  0.1f, 0),
        new Vector3(-0.1f, 0, -0.1f),

        new Vector3(0.18f, -0.1f, 0.1f),
        new Vector3(0.18f, 0.1f, 0.1f),
        new Vector3(0.18f, -0.1f, -0.1f),
        new Vector3(0.18f, 0.1f, -0.1f),
        new Vector3(-0.1f, -0.1f, 0.1f),
        new Vector3(-0.1f, 0.1f, 0.1f),
        new Vector3(-0.1f, -0.1f, -0.1f),
        new Vector3(-0.1f, 0.1f, -0.1f),
    };

    public static Vector3[] PlayerRightArmRotation =
    {
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),

        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
    };
    #endregion

    #region 左腕のパーツ装着座標と回転
    public static Vector3[] PlayerLeftArmPosition =
    {
        new Vector3(0.18f, 0, 0.1f),
        new Vector3(0.18f, -0.1f, 0),
        new Vector3(0.18f,  0.1f, 0),
        new Vector3(0.18f, 0, -0.1f),
        new Vector3(-0.1f, 0, 0.1f),
        new Vector3(-0.1f, -0.1f, 0),
        new Vector3(-0.1f,  0.1f, 0),
        new Vector3(-0.1f, 0, -0.1f),

        new Vector3(0.18f, -0.1f, 0.1f),
        new Vector3(0.18f, 0.1f, 0.1f),
        new Vector3(0.18f, -0.1f, -0.1f),
        new Vector3(0.18f, 0.1f, -0.1f),
        new Vector3(-0.1f, -0.1f, 0.1f),
        new Vector3(-0.1f, 0.1f, 0.1f),
        new Vector3(-0.1f, -0.1f, -0.1f),
        new Vector3(-0.1f, 0.1f, -0.1f),
    };

    public static Vector3[] PlayerLeftArmRotation =
    {
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),

        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
    };
    #endregion

    #region 右脚のパーツ装着座標と回転
    public static Vector3[] PlayerRightLegPosition =
    {
        new Vector3(0.226f, -0.04f, 0.08f),
        new Vector3(0.18f, -0.1f, 0),
        new Vector3(0.18f,  0.1f, 0),
        new Vector3(0.18f, 0, -0.1f),
        new Vector3(-0.1f, 0, 0.1f),
        new Vector3(-0.1f, -0.1f, 0),
        new Vector3(-0.1f,  0.1f, 0),
        new Vector3(-0.1f, 0, -0.1f),

        new Vector3(0.18f, -0.1f, 0.1f),
        new Vector3(0.18f, 0.1f, 0.1f),
        new Vector3(0.18f, -0.1f, -0.1f),
        new Vector3(0.18f, 0.1f, -0.1f),
        new Vector3(-0.1f, -0.1f, 0.1f),
        new Vector3(-0.1f, 0.1f, 0.1f),
        new Vector3(-0.1f, -0.1f, -0.1f),
        new Vector3(-0.1f, 0.1f, -0.1f),
    };

    public static Vector3[] PlayerRightLegRotation =
    {
        new Vector3(90.0f,  0.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),

        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
    };
    #endregion

    #region 左脚のパーツ装着座標と回転
    public static Vector3[] PlayerLeftLegPosition =
    {
        new Vector3(-0.226f, -0.04f, 0.08f),
        new Vector3(0.18f, -0.1f, 0),
        new Vector3(0.18f,  0.1f, 0),
        new Vector3(0.18f, 0, -0.1f),
        new Vector3(-0.1f, 0, 0.1f),
        new Vector3(-0.1f, -0.1f, 0),
        new Vector3(-0.1f,  0.1f, 0),
        new Vector3(-0.1f, 0, -0.1f),

        new Vector3(0.18f, -0.1f, 0.1f),
        new Vector3(0.18f, 0.1f, 0.1f),
        new Vector3(0.18f, -0.1f, -0.1f),
        new Vector3(0.18f, 0.1f, -0.1f),
        new Vector3(-0.1f, -0.1f, 0.1f),
        new Vector3(-0.1f, 0.1f, 0.1f),
        new Vector3(-0.1f, -0.1f, -0.1f),
        new Vector3(-0.1f, 0.1f, -0.1f),
    };

    public static Vector3[] PlayerLeftLegRotation =
    {
        new Vector3(90.0f,  0.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),
        new Vector3(180.0f, -90.0f, 0),
        new Vector3(180.0f, -90.0f, 90),
        new Vector3(180.0f, -90.0f, -90),
        new Vector3(180.0f, -90.0f, 180),

        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, -45),
    };
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
}	}

