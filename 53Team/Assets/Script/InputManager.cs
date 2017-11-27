using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private GameObject _player;
    private Player _partsPurge;

    private float _pushTime;
    [System.NonSerialized] public  bool _longPush = false;
    [SerializeField] PlayerStatusCheck _playerUIManager;


    // Use this for initialization
    void Start()
    {
        _player = GameObject.Find("PlayerParent");
        _partsPurge = _player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // 部分パージのInput    
        // 右パージ 
        if(Input.GetAxisRaw("crossX") > 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _partsPurge._rightArmParge == true)
            {
                _partsPurge._rightArmParge = false;

                if (_partsPurge._charaPara._rightArm_AttackState == Weapon.Attack_State.shooting)
                {
                    _partsPurge._specialWepon_Shot[0].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                 {
                     _partsPurge.PargeAttackCollide(false, CharaBase.Parts.RightArm);
                     PartsHPUIReset(CharaBase.Parts.RightArm);
                 });
                }

                else
                {
                    _partsPurge.PargeAttackCollide(false, CharaBase.Parts.RightArm);
                    PartsHPUIReset(CharaBase.Parts.RightArm);
                }
                
            }
        }
        // 左パージ
        if (Input.GetAxisRaw("crossX") < 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _partsPurge._leftArmParge == true)
            {
                _partsPurge._leftArmParge = false;
                if (_partsPurge._charaPara._leftArm_AttackState == Weapon.Attack_State.shooting)
                {
                    _partsPurge._specialWepon_Shot[1].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.LeftArm);
                        PartsHPUIReset(CharaBase.Parts.LeftArm);
                    });
                }
                else
                {
                    _partsPurge.PargeAttackCollide(false, CharaBase.Parts.LeftArm);
                    PartsHPUIReset(CharaBase.Parts.LeftArm);
                }
            }
        }
        // 足パージ
        if (Input.GetAxisRaw("crossY") < 0)
        {
            _pushTime += Time.deltaTime;
            if (_longPush == true && _partsPurge._legParge == true)
            {
                _partsPurge._legParge = false;
                if (_partsPurge._charaPara._leg_AttackState == Weapon.Attack_State.shooting)
                {
                    _partsPurge._specialWepon_Shot[2].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.Leg);
                        PartsHPUIReset(CharaBase.Parts.Leg);
                    });
                }
                else
                {
                    _partsPurge.PargeAttackCollide(false, CharaBase.Parts.Leg);
                    PartsHPUIReset(CharaBase.Parts.Leg);
                }
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

    private void PartsHPUIReset(Player.Parts parts)
    {
        _playerUIManager.ArmorHP((int)parts, _partsPurge._partsHP[(int)parts], _partsPurge._partsMaxHP[(int)parts]);
    }
}
