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
        SQUAT,
        Ability,
        SKILL,
        DIE
    }
    private playerState _status = playerState.IDLE;
    

    // Use this for initialization
    void Start ()
    {
        _status = playerState.IDLE;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public playerState PlayerState
    {
        get { return _status; }
        set { _status = value; }
    }

}
