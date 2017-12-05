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
                else if (_partsPurge._charaPara._rightArm_AttackState == Weapon.Attack_State.approach)
                {
                    _partsPurge._specialWepon_Approach[0].GetComponent<PargeApproach>().PargeAttack(1500, () =>
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
                else if (_partsPurge._charaPara._leftArm_AttackState == Weapon.Attack_State.approach)
                {
                    _partsPurge._specialWepon_Approach[1].GetComponent<PargeApproach>().PargeAttack(1500, () =>
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
                else if (_partsPurge._charaPara._leg_AttackState == Weapon.Attack_State.approach)
                {
                    _partsPurge._specialWepon_Approach[2].GetComponent<PargeApproach>().PargeAttack(1500, () =>
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
    }

    private void PartsHPUIReset(Player.Parts parts)
    {
        _playerUIManager.ArmorHP((int)parts, _partsPurge._partsHP[(int)parts], _partsPurge._partsMaxHP[(int)parts]);
    }
}
