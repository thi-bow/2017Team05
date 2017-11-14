using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
                    }
                    else
                    {
                        Damage(n.value);
                    }

                });
            }
        }
    }

    // Update is called once per frame
    protected override void Update ()
    {

        if (Input.GetButtonDown("Parge"))
        {
            FullParge(ArmorParge);
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
            }
            if (Input.GetAxis("crossX") < 0)
            {
                PartsAdd(Parts.LeftArm, other.GetComponent<Armor>());
            }
            if (Input.GetAxis("crossY") > 0)
            {
                PartsAdd(Parts.Leg, other.GetComponent<Armor>());
            }
        }
    }

    public void ArmorParge()
    {
        parge = true;
        _pargeColl.radius += Time.deltaTime;
        if (_pargeColl.radius <= 2.0f)
        {
            parge = false;
        }
    }
}
