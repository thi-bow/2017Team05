using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkyMove : MonoBehaviour
{
    [SerializeField] private Player _player;
    private GameObject _mainCamera = null;
    private Rigidbody _myRigidbody = null;
    Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
    
    [Header("----------------移動速度---------------------")]
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _boostPower = 2.0f;
    [SerializeField] private float _downSpeed = 2.0f;
    [System.NonSerialized] public Vector3 boostVelocity = Vector3.zero;
    public bool _useBoostFlg = false;
    [System.NonSerialized] public float _maxBosstGage = 0.0f;
    [SerializeField] private  float _boostGage = 100.0f;
    private bool _boostParge = false;
    private float _pargeCount = 0.0f;

    // Use this for initialization
    void Start ()
    {
        _mainCamera = _player._mainCamera;
        _myRigidbody = _player.GetComponent<Rigidbody>();
        _maxBosstGage = _boostGage;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SkyMove()
    {
        if(_boostParge)
        {
            PargeSkyMove();
            return;
        }
        
        if(Input.GetButtonDown("Jump") && _boostGage > 0 )
        {
            _useBoostFlg = !_useBoostFlg;
        }

        var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");

        if (_player._charaPara._boosterLevel <= 1)
        {
            _move *= 0.0f;
        }

        else if (_player._charaPara._boosterLevel <= 2)
        {
            _move *= 0.5f;
        }
        else
        {
            print("ブーストレベル3");
            _move *= 1.0f;
        }

        //上昇Flagが立っている間は上昇、それ以外は下降する
        if (_useBoostFlg)
        {
            if (_player._charaPara._boosterLevel >= 2)
            {
                _move += new Vector3(0, _boostPower, 0);
            }
            else if (boostVelocity.y >= 0)
            {
                _move += new Vector3(0, _boostPower, 0) + (boostVelocity * 0.1f);
            }
            else
            {
                _move += new Vector3(0, _boostPower, 0) + new Vector3(boostVelocity.x, 0.0f, boostVelocity.z) * 0.03f;
            }
            Debug.Log(_myRigidbody.velocity * 0.9f);
            _boostGage -= 1.0f;
            if(_boostGage <= 0)
            {
                _useBoostFlg = false;
                _boostGage = 0;
            }
        }
        else
        {

            if (_player._charaPara._boosterLevel >= 2)
            {
                _move += new Vector3(0, _downSpeed, 0);
            }
            else
            {
                _move += new Vector3(0, _downSpeed, 0) + new Vector3(boostVelocity.x, 0.0f, boostVelocity.z) * 0.03f;
            }
        }
        _move *= _moveSpeed;
        _myRigidbody.MovePosition(_player.transform.localPosition + _move);
    }

    void PargeSkyMove()
    {
        var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        _move += new Vector3(0, _boostPower, 0) + new Vector3(boostVelocity.x, 0.0f, boostVelocity.z) * 0.03f;
        _move *= _moveSpeed;
        _myRigidbody.MovePosition(_player.transform.localPosition + _move);
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
