using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum playerState
    {
        IDLE,
        MOVE,
        RUN,
        ATTACK,
        SQUAT,
        DIE
    }
    public enum playerSkill
    {
        NONE,
        Ability,
        SKILL,
    }

    public playerState _status = playerState.IDLE;
    public playerSkill _skillStatus = playerSkill.NONE;
    private PlayerMove _playerMove = null;
    

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

}
