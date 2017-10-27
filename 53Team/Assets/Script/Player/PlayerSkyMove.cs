﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkyMove : MonoBehaviour
{
    private GameObject _parent = null;
    [SerializeField] private Player _player;
    private GameObject _mainCamera = null;
    private Rigidbody _myRigidbody = null;
    Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
    
    [Header("----------------移動速度---------------------")]
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _boostPower = 2.0f;
    [SerializeField] private float _downSpeed = 2.0f;
    public bool _useBoostFlg = false;
    [SerializeField] private  float _boostGage = 100.0f;

    // Use this for initialization
    void Start ()
    {
        _parent = this.transform.parent.gameObject;
        _mainCamera = _player._mainCamera;
        _myRigidbody = _parent.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SkyMove()
    {
        if(Input.GetButtonDown("Jump"))
        {
            _useBoostFlg = !_useBoostFlg;
        }

        var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");

        if (_player.BoosterArmorList.Count <= 1)
        {
            _move *= 0.0f;
        }

        else if (_player.BoosterArmorList.Count <= 2)
        {
            _move *= _moveSpeed * 0.5f;
        }
        else
        {
            _move *= _moveSpeed;
        }

        //上昇Flagが立っている間は上昇、それ以外は下降する
        if (_useBoostFlg)
        {
            _move += new Vector3(0, _boostPower, 0) + (_myRigidbody.velocity * 0.9f);
            _boostGage -= 1.0f;
            if(_boostGage <= 0)
            {
                _useBoostFlg = false;
                _boostGage = 0;
            }
        }
        else
        {
            _move += new Vector3(0, _downSpeed, 0);
        }

        _myRigidbody.MovePosition(_parent.transform.localPosition + _move);
    }

    public bool UseBoost
    {
        get { return _useBoostFlg; }
        set { _useBoostFlg = value; }
    }

    public float BoostGage
    {
        get { return _boostGage; }
        set { _boostGage = value; }
    }

}