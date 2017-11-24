using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private GameObject _player;
    private CharaBase _partsPurge;

    private float _pushTime;
    [System.NonSerialized] public  bool _longPush = false;


    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("PlayerParent");
        _partsPurge = _player.GetComponent<CharaBase>();
    }

    // Update is called once per frame
    void Update()
    {
        // 部分パージのInput    
        // 左パージ 
        if(Input.GetAxisRaw("crossX") > 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _player.GetComponent<Player>()._rightArmParge == true)
            {
                _player.GetComponent<Player>()._leftArmParge = false;
                _partsPurge._specialWepon_Shot[1].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera,()=> 
                {
                    _partsPurge.BrowOffParge(CharaBase.Parts.RightArm);
                });
            }
        }
        // 右パージ
        if (Input.GetAxisRaw("crossX") < 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _player.GetComponent<Player>()._leftArmParge == true)
            {
                _player.GetComponent<Player>()._leftArmParge = false;
                _partsPurge._specialWepon_Shot[0].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                {
                    _partsPurge.BrowOffParge(CharaBase.Parts.LeftArm);
                });
            }
        }
        // 足パージ
        if (Input.GetAxisRaw("crossY") < 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _player.GetComponent<Player>()._legParge == true)
            {
                _player.GetComponent<Player>()._legParge = false;
                _partsPurge._specialWepon_Shot[2].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                {
                    _partsPurge.BrowOffParge(CharaBase.Parts.Leg);
                });
            }
        }
        // フルパージボタン
        if (Input.GetAxisRaw("crossY") > 0)
        {
           _pushTime += Time.deltaTime;
            Debug.Log(_longPush);
        }

        // タイマーのリセット
        if (Input.GetAxisRaw("crossX") == 0 && Input.GetAxisRaw("crossY") == 0)
        {
            _pushTime = 0;
            _longPush = false;
        }
        // 長押しフラグの切り替え
        if (_pushTime >= 0.3f)
        {
            _longPush = true;
        }



        //// コントローラーInputのデバック
        //// パージ
        //if (Input.GetButton("Parge"))
        //{
        //    Debug.Log("Parge");
        //}
        //// 左近接
        //if (Input.GetButtonDown("LeftArmStrike"))
        //{
        //    Debug.Log("LeftArmStrike");
        //}
        //// 右近接
        //if (Input.GetButtonDown("RightArmStrike"))
        //{
        //    Debug.Log("RightArmStrike");
        //}
        //// 射撃攻撃
        //// 左
        //if (Input.GetAxisRaw("ArmShot") == 1)
        //{
        //    Debug.Log("ArmShot01");
        //}
        //// 右
        //else if(Input.GetAxisRaw("ArmShot") == -1)
        //{
        //    Debug.Log("ArmShot-01");
        //}
        //// 走る
        //if (Input.GetButton("Run"))
        //{
        //    Debug.Log("Run");
        //}
        //// 足攻撃
        //if(Input.GetButton("LegAttack"))
        //{
        //    Debug.Log("LegAttack");
        //}
        //// バックボタン
        //if (Input.GetButton("BackButton"))
        //{
        //    Debug.Log("BackButton");
        //}
        //// しゃがみ
        //if (Input.GetButton("Squat"))
        //{
        //    Debug.Log("Squat");
        //}

        //// 装備ボタン
        //// 左
        //if (Input.GetAxisRaw("crossX") == 1)
        //{
        //    Debug.Log("crossX01");
        //}
        //// 右
        //else if (Input.GetAxisRaw("crossX") == -1)
        //{
        //    Debug.Log("crossX-01");
        //}
        //// 上
        //if (Input.GetAxisRaw("crossY") == 1)
        //{
        //    Debug.Log("crossY01");
        //}
        //// 下
        //else if (Input.GetAxisRaw("crossY") == -1)
        //{
        //    Debug.Log("crossY-01");
        //}
    }
}
