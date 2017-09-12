using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player _player = null; 
    [SerializeField] private GameObject _mainCamera = null;

    #region 移動に関する変数
    private Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _moveSpeed_Run = 2.0f;
    #endregion


    // Use this for initialization
    void Start () {
        _player = this.gameObject.GetComponent<Player>();
		
	}
	
	// Update is called once per frame
	void Update () {
        //移動状態ならば
        //if (_player.PlayerState == Player.playerState.MOVE || _player.PlayerState == Player.playerState.RUN)
        //{
            var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
            //if (_player.PlayerState == Player.playerState.MOVE)
            //{
                _move *= _moveSpeed;
            //}
            //else
            //{
            //    _move *= _moveSpeed_Run;
            //}
            this.transform.localPosition += _move;
        //}

    }
}
