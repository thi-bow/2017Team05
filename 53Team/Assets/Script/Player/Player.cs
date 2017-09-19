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
    #region プレイヤーの状態に関する変数
    [SerializeField] private playerState _status = playerState.IDLE;
    public playerSkill _skillStatus = playerSkill.NONE;
    private bool _attackPlay = false;
    #endregion
    

    // Use this for initialization
    void Start ()
    {
        _playerMove = this.gameObject.GetComponent<PlayerMove>();
        _status = playerState.IDLE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _playerMove.Move();
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

}
