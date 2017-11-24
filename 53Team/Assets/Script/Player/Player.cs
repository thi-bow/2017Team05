﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class Player : CharaBase
{
    public enum playerState
    {
        IDLE,
        MOVE,
        RUN,
        SQUAT,
        SKYMOVE,
        DIE
    }
    public enum playerSkill
    {
        NONE,
        Ability,
        SKILL,
    }

    [Header("プレイヤーのパラメーター")]
    [SerializeField]
    private GameObject _playerChild = null;
    private PlayerMove _playerMove = null;
    private PlayerSkyMove _playerSkyMove = null;
    public GameObject _mainCamera = null;
    #region プレイヤーの状態に関する変数
    [SerializeField] private playerState _status = playerState.IDLE;
    public playerSkill _skillStatus = playerSkill.NONE;
    private bool _attackPlay = false;
    #endregion
    
    [SerializeField]  private SphereCollider _pargeColl;
    private bool parge = false;
    [SerializeField] BoneCollide[] _boneCollide;
    [SerializeField] PlayerStatusCheck _playerUIManager;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        _playerMove = _playerChild.GetComponent<PlayerMove>();
        _playerSkyMove = _playerChild.GetComponent<PlayerSkyMove>();
        _status = playerState.IDLE;

        if (_boneCollide.Length > 0)
        {
            for (int i = 0; i < _boneCollide.Length; i++)
            {
                int number = i;
                _boneCollide[number].OnDamage.Subscribe(n =>
                {
                    Parts par = _boneCollide[number].m_parts;
                    if (n.type == Weapon.Attack_State.shooting)
                    {
                        PartsDamage(n.value, par);
                        _playerUIManager.ArmorHP((int)par, _partsHP[(int)par]);
                        Debug.Log(_partsHP[(int)par]);
                    }
                    else
                    {
                        Damage(n.value);
                    }
                    _playerUIManager.PlayerHP();
                });
            }
        }
        _deadAction = DeadAction;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        //右腕の攻撃
        if(Input.GetAxis("ArmShot") > 0.5f )
        {
            RightArmtShot();
            Debug.Log("右腕で攻撃ほげほげ");
        }
        else if(Input.GetButtonDown("RightArmStrike"))
        {
            int attack = _charaPara._rightAttack;
            if (_charaPara._rightArm_AttackState == Weapon.Attack_State.approach)
            {
                attack *= 2;
            }
            this.GetComponent<ApproachAttack>().Approach(attack);
        }

        //左腕の攻撃
        if (Input.GetAxis("ArmShot") < -0.5f)
        {
            LeftArmShot();
        }
        else if (Input.GetButtonDown("LeftArmStrike"))
        {
            int attack = _charaPara._leftAttack;
            if (_charaPara._leftArm_AttackState == Weapon.Attack_State.approach)
            {
                attack *= 2;
            }
            this.GetComponent<ApproachAttack>().Approach(attack);
        }

        //脚の攻撃
        if(Input.GetButton("LegAttack"))
        {
            LegShot();
        }


        if (Input.GetButtonDown("Parge") && _fullParge)
        {
            FullParge(() => {
                int attackPower = 0;
                for (int i = 0; i < _allPartsList.Count; i++)
                {
                     attackPower += GetPartsList(_allPartsList[i]).Count * 100;
                }
                PargeAttackCollide(attackPower, true);
            });
        }

        base.Update();
    }

    private void LateUpdate()
    {
        if (_status == playerState.SKYMOVE)
        {
            _playerSkyMove.SkyMove();
        }
        else
        {
            _playerMove.Move();
            if (_playerSkyMove.BoostGage < 100)
            {
                _playerSkyMove.BoostGage += 1.0f;
            }

            if (_playerSkyMove.BoostGage < _playerSkyMove._maxBosstGage)
            {
                _playerSkyMove.BoostGage += 1.0f;
            }
        }
    }

    //プレイヤーの状態
    public playerState PlayerState
    {
        get { return _status; }
        set { _status = value; }
    }

    //プレイヤーの攻撃状態
    public bool AttackCheck
    {
        get { return _attackPlay; }
        set { _attackPlay = value; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //地面に触っていれば空中にいる判定は消す
        if(collision.gameObject.tag == "Ground")
        {
            if(_playerMove.JumpFlg == true)
            {
                _playerMove.JumpFlg = false;
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            if(_status == playerState.SKYMOVE)
            {
                _status = playerState.MOVE;
                _playerSkyMove.UseBoost = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //地面に触っていれば空中にいる判定は消す
        if (other.tag == "Ground")
        {
            if (_playerMove.JumpFlg == true)
            {
                _playerMove.JumpFlg = false;
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            if (_status == playerState.SKYMOVE)
            {
                _status = playerState.MOVE;
                _playerSkyMove.UseBoost = false;
            }
        }
        //落ちている武器に当たれば、その武器を装着する
        if (other.tag == "Armor")
        {
            if (other.GetComponent<Armor>().GetParts == Parts.Body || other.GetComponent<Armor>().GetParts == Parts.Booster)
            {
                PartsAdd(other.GetComponent<Armor>().GetParts, other.GetComponent<Armor>());
                _playerUIManager.ArmorHP((int)other.GetComponent<Armor>().GetParts, _partsHP[(int)other.GetComponent<Armor>().GetParts]);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //落ちている武器に当たれば、その武器を装着する
        if (other.tag == "Armor" && other.GetComponent<Armor>().GetParts != Parts.Body && other.GetComponent<Armor>().GetParts != Parts.Booster)
        {
            if (Input.GetAxis("crossX") > 0)
            {
                PartsAdd(Parts.RightArm, other.GetComponent<Armor>());
                _playerUIManager.ArmorHP((int)Parts.RightArm, _partsHP[(int)Parts.RightArm]);
            }
            if (Input.GetAxis("crossX") < 0)
            {
                PartsAdd(Parts.LeftArm, other.GetComponent<Armor>());
                _playerUIManager.ArmorHP((int)Parts.LeftArm, _partsHP[(int)Parts.LeftArm]);
            }
            if (Input.GetAxis("crossY") > 0)
            {
                PartsAdd(Parts.Leg, other.GetComponent<Armor>());
                _playerUIManager.ArmorHP((int)Parts.Leg, _partsHP[(int)Parts.Leg]);
            }
        }
    }


    //必殺技をやるときはこの関数を呼ぶ
    public void ArmorParge(Parts parts, Action action)
    {
        //この中にパージしたときの必殺技処理を入れる
        action();
        //
        BrowOffParge(parts);
    }

    public void PargeAttackCollide(int attackPower, bool fullParge = false)
    {
        float maxSize = 2.0f;
        _pargeColl.gameObject.SetActive(true);
        if (fullParge) maxSize = 5.0f;
        _pargeColl.GetComponent<PargeAttackCollider>().PargeStart(attackPower, maxSize);

    }

    void DeadAction()
    {
        SceneManagerScript.sceneManager.SceneOut("CheckScene");
    }
}
