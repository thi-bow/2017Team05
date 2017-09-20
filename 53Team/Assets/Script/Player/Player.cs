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

    private PlayerMove _playerMove = null;
    private PlayerSkyMove _playerSkyMove = null;
    public GameObject _mainCamera = null;
    #region プレイヤーの状態に関する変数
    [SerializeField] private playerState _status = playerState.IDLE;
    public playerSkill _skillStatus = playerSkill.NONE;
    private bool _attackPlay = false;
    #endregion
    

    // Use this for initialization
    void Start ()
    {
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _playerSkyMove = this.gameObject.GetComponent<PlayerSkyMove>();
        _status = playerState.IDLE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_status == playerState.SKYMOVE)
        {
            print("空中移動");
            _playerSkyMove.SkyMove();
        }
        else
        {
            _playerMove.Move();
        }
    }

    public playerState PlayerState
    {
        get { return _status; }
        set { _status = value; }
    }

    public bool AttackCheck
    {
        get { return _attackPlay; }
        set { _attackPlay = value; }
    }

    private void OnCollisionEnter(Collision collision)
    {
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
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("aaaaaaaaaaaaaa");
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
            }
        }
    }





}
