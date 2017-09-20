using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkyMove : MonoBehaviour
{
    private Player _player;
    private GameObject _mainCamera = null;

    [Header("----------------移動速度---------------------")]
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _boostPower = 2.0f;
    [SerializeField] private float _downSpeed = 2.0f;
    Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);

    // Use this for initialization
    void Start ()
    {
        _player = this.gameObject.GetComponent<Player>();
        _mainCamera = _player._mainCamera;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SkyMove()
    {
        var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");

        if(Input.GetButton("Jump"))
        {
            _move += new Vector3(0, _boostPower, 0);
        }
        else
        {
            _move += new Vector3(0, _downSpeed, 0);
        }

        _move *= _moveSpeed;
        this.transform.localPosition += _move;
    }
}
