using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerChild;
    private Animator mAnimator;

    private Player _partsPurge;

    private float _pushTime;
    [System.NonSerialized] public  bool _longPush = false;
    [SerializeField] PlayerStatusCheck _playerUIManager;

    private float _soundSpan = 0.55f;

    // Use this for initialization
    void Start()
    {
        mAnimator = _playerChild.GetComponent<Animator>();
        _partsPurge = _player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _soundSpan -= Time.deltaTime;
        // 部分パージのInput    
        // 右パージ 
        if (Input.GetAxisRaw("crossX") > 0 && !TutorialManager._purgeOff)
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
        if (Input.GetAxisRaw("crossX") < 0 && !TutorialManager._purgeOff)
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
        if (Input.GetAxisRaw("crossY") < 0 && !TutorialManager._purgeOff)
        {
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
        if (Input.GetAxisRaw("crossY") > 0 && !TutorialManager._purgeOff)
        {
           _pushTime += Time.deltaTime;
            Debug.Log(_longPush);
        }

        // タイマーのリセット
        if (Input.GetAxisRaw("crossX") == 0 && Input.GetAxisRaw("crossY") == 0)
        {
            _pushTime = 0;
            _longPush = false;
            mAnimator.SetBool("walk", false);
        }

        // 歩きアニメーションとSE
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            mAnimator.SetBool("walk", false);
        }
        else
        {
            _pushTime += Time.deltaTime;
            // 歩きアニメーション
            mAnimator.SetBool("walk", true);
        }

        if (mAnimator.GetBool("walk") == true && mAnimator.GetBool("run") == false && mAnimator.GetBool("ground") == false)
        {
            if (_soundSpan <= 0)
            {
                // 歩きSE
                SoundManger.Instance.PlaySE(25);
                _soundSpan = 0.55f;
            }
        }
        if (mAnimator.GetBool("run") == true && mAnimator.GetBool("ground") == false)
        {
            if (_soundSpan <= 0)
            {
                // 走りSE
                SoundManger.Instance.PlaySE(25);
                _soundSpan = 0.256f;
            }
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