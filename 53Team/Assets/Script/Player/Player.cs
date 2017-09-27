using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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
    
    [SerializeField] private PlayerMove _playerMove = null;
    [SerializeField] private PlayerSkyMove _playerSkyMove = null;
    public GameObject _mainCamera = null;
    #region プレイヤーの状態に関する変数
    [SerializeField] private playerState _status = playerState.IDLE;
    public playerSkill _skillStatus = playerSkill.NONE;
    private bool _attackPlay = false;
    #endregion
    

    // Use this for initialization
    void Start ()
    {
        _status = playerState.IDLE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_status == playerState.SKYMOVE)
        {
            _playerSkyMove.SkyMove();
        }
        else
        {
            _playerMove.Move();
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
    }





}
