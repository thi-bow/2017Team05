using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerChild;
    [SerializeField] private GameObject _cameraLock;
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
                    // PlayerMove無効化
                    _player.GetComponent<Player>().enabled = false;
                    // パージアニメーション再生
                    mAnimator.SetBool("rightparge", true);
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Shot[0].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.RightArm);
                         PartsHPUIReset(CharaBase.Parts.RightArm);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
                        // パージアニメーション停止
                        mAnimator.SetBool("rightparge", false);
                    });
                }
                else if (_partsPurge._charaPara._rightArm_AttackState == Weapon.Attack_State.approach)
                {
                    // PlayerMove無効化
                    _player.GetComponent<Player>().enabled = false;
                    // パージアニメーション再生
                    mAnimator.SetBool("rightparge", true);
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Approach[0].GetComponent<PargeApproach>().PargeAttack(1500, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.RightArm);
                        PartsHPUIReset(CharaBase.Parts.RightArm);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
                        // パージアニメーション停止
                        mAnimator.SetBool("rightparge", false);
                    });
                }
                else
                {
                    _partsPurge.PargeAttackCollide(false, CharaBase.Parts.RightArm);
                    PartsHPUIReset(CharaBase.Parts.RightArm);
                    // パージアニメーション停止
                    mAnimator.SetBool("rightparge", false);
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
                    // PlayerMove有効化
                    _player.GetComponent<Player>().enabled = false;
                    // パージアニメーション再生
                    mAnimator.SetBool("leftparge", true);
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Shot[1].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.LeftArm);
                        PartsHPUIReset(CharaBase.Parts.LeftArm);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
                        // パージアニメーション停止
                        mAnimator.SetBool("leftparge", false);
                    });
                }
                else if (_partsPurge._charaPara._leftArm_AttackState == Weapon.Attack_State.approach)
                {
                    // PlayerMove無効化
                    _player.GetComponent<Player>().enabled = false;
                    // パージアニメーション再生
                    mAnimator.SetBool("leftparge", true);
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Approach[1].GetComponent<PargeApproach>().PargeAttack(1500, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.LeftArm);
                        PartsHPUIReset(CharaBase.Parts.LeftArm);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
                        // パージアニメーション停止
                        mAnimator.SetBool("leftparge", false);
                    });
                }
                else
                {
                    _partsPurge.PargeAttackCollide(false, CharaBase.Parts.LeftArm);
                    PartsHPUIReset(CharaBase.Parts.LeftArm);
                    // パージアニメーション停止
                    mAnimator.SetBool("leftparge", false);
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
                    // PlayerMove無効化
                    _player.GetComponent<Player>().enabled = false;
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Shot[2].GetComponent<PargeShot>().PargeAttack(_partsPurge.tpsCamera, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.Leg);
                        PartsHPUIReset(CharaBase.Parts.Leg);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
                    });
                }
                else if (_partsPurge._charaPara._leg_AttackState == Weapon.Attack_State.approach)
                {
                    // PlayerMove無効化
                    _playerChild.GetComponent<PlayerMove>().enabled = false;
                    // カメラ操作無効化
                    _cameraLock.GetComponent<TPS_Camera>().enabled = false;
                    _partsPurge._specialWepon_Approach[2].GetComponent<PargeApproach>().PargeAttack(1500, () =>
                    {
                        _partsPurge.PargeAttackCollide(false, CharaBase.Parts.Leg);
                        PartsHPUIReset(CharaBase.Parts.Leg);
                        // PlayerMove有効化
                        _player.GetComponent<Player>().enabled = true;
                        // カメラ操作有効化
                        _cameraLock.GetComponent<TPS_Camera>().enabled = true;
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
                _soundSpan = 0.686f;
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
        if(Input.GetButton("Pause") == true && Input.GetButton("BackButton") == true)
        {
            // 強制終了処理

        }
        //カーソル非表示
        Cursor.visible = false;
    }

    private void PartsHPUIReset(Player.Parts parts)
    {
        _playerUIManager.ArmorHP((int)parts, _partsPurge._partsHP[(int)parts], _partsPurge._partsMaxHP[(int)parts]);
    }
}