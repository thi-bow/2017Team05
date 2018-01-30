using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartsAddPara : MonoBehaviour
{

    #region 右腕のパーツ装着座標と回転
    public static Vector3[] PlayerRightArmPosition =
    {
        new Vector3(0.434f, 0, 0.1f),
        new Vector3(0.434f, -0.0491f, 0.114f),
        new Vector3(0.43f,  -0.0491f, -0.13f),
        new Vector3(0.446f, -0.182f, -0.022f),
        new Vector3(0.438f, 0.101f, -0.021f),
        new Vector3(0.444f, -0.177f, 0.12f),
        new Vector3(0.448f, -0.177f, -0.136f),
        new Vector3(0.436f, 0.111f, 0.121f),

        new Vector3(0.448f, 0.111f, -0.136f),
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
        new Vector3(180.0f, -90.0f, -45),
        new Vector3(180.0f, -90.0f, 45),
        new Vector3(180.0f, -90.0f, 45),

        new Vector3(180.0f, -90.0f, -45),
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
        new Vector3(-0.15f, -0.43f,  0.1f),
        new Vector3(-0.268f, -0.43f, 0.1f),
        new Vector3(-0.047f, -0.43f, 0.1f),
        new Vector3(-0.15f, -0.43f,  0.244f),
        new Vector3(-0.15f, -0.43f,  0.001f),
        new Vector3(-0.268f, -0.43f, 0.244f),
        new Vector3(-0.268f, -0.43f, 0.001f),
        new Vector3(-0.047f, -0.43f, 0.244f),

        new Vector3(-0.047f, -0.43f,  0.001f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
        new Vector3(-0.15f, -0.43f, -0.015f),
    };

    public static Vector3[] PlayerLeftArmRotation =
    {
        new Vector3(-270.0f, 0, 90.0f),
        new Vector3(-270.0f, 0, 90),
        new Vector3(-270.0f, 0, 90),
        new Vector3(-270.0f, 90.0f, 90),
        new Vector3(-270.0f, 90.0f, 90),
        new Vector3(-270.0f, 0.0f, 45),
        new Vector3(-270.0f, 0.0f, -45),
        new Vector3(-270.0f, 0.0f, -45),

        new Vector3(270.0f,  0.0f, 45),
        new Vector3(270.0f, -90.0f, -45),
        new Vector3(270.0f, -90.0f, 45),
        new Vector3(270.0f, -90.0f, -45),
        new Vector3(270.0f, -90.0f, 45),
        new Vector3(270.0f, -90.0f, -45),
        new Vector3(270.0f, -90.0f, 45),
        new Vector3(270.0f, -90.0f, -45),
    };
    #endregion

    #region 右脚のパーツ装着座標と回転
    public static Vector3[] PlayerRightLegPosition =
    {
        new Vector3(-0.274f, -0.227f, 0.042f),
        new Vector3(-0.087f, -0.215f, 0.107f),
        new Vector3(0.095f,  -0.114f, 0.141f),
        new Vector3(0.141f, -0.134f, 0.154f),
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
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
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
        new Vector3(0.274f, -0.227f, 0.091f),
        new Vector3(0.087f, -0.215f, 0.091f),
        new Vector3(-0.095f,  -0.114f, 0.159f),
        new Vector3(0.159f, -0.134f, 0.166f),
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
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
        new Vector3(90.0f,  0.0f, 90),
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

