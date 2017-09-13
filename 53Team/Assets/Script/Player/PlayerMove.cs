using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player _player = null; 
    [SerializeField] private GameObject _mainCamera = null;

    [Header("----------------移動速度---------------------")]
    #region 移動に関する変数
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _moveSpeed_Run = 2.0f;
    [SerializeField] private float _moveSpeed_Squat = 0.5f;
    private bool _squatflg = false;
    private Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
    #endregion

    [Header("--------------特殊行動の詳細------------------")]
    #region 特殊移動に関する変数
    [SerializeField] private float _slidingtime = 1.0f;
    [SerializeField] private float _rollingtime = 1.0f;
    [SerializeField] private float _smallColliderHeight = 1.0f;
    [SerializeField] private float _smallColliderPositionY = -0.5f;
    private bool _actionFlg = false;
    #endregion



    // Use this for initialization
    void Start () {
        _player = this.gameObject.GetComponent<Player>();
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Move()
    {
        //スライディング中なら移動以外の入力を受け付けない
        if (!_actionFlg)
        {
            //走るかどうか
            if (Input.GetButton("Run") && !_player.AttackCheck)
            {
                //しゃがんでいたら立ち上がり走る
                if (_player.PlayerState == Player.playerState.SQUAT)
                {
                    Squat(false, Player.playerState.RUN);
                }
                else
                {
                    _player.PlayerState = Player.playerState.RUN;
                }
            }
            //しゃがんでいなかったら歩く
            else if (_player.PlayerState != Player.playerState.SQUAT)
            {
                _player.PlayerState = Player.playerState.MOVE;
            }

            var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");


            //回避ボタンを押したときの処理
            if (Input.GetKeyDown(KeyCode.T))
            {
                float _moveCheck = 0.0f;
                _moveCheck = _move.x + _move.z;

                //スライディング
                if ((_moveCheck >= 0.5f || _moveCheck <= -0.5f) && _player.PlayerState == Player.playerState.RUN)
                {
                    print("スライディング反応");
                    Sliding();
                }
                //回転回避
                else if (Input.GetKeyDown(KeyCode.T) &&
                    (_player.PlayerState == Player.playerState.MOVE || _player.PlayerState == Player.playerState.SQUAT))
                {
                    print("回転回避");
                    Rolling();
                }
            }
            
            //走っていないときにしゃがむボタンを押すとしゃがむ
            if (Input.GetKeyDown(KeyCode.J) && _player.PlayerState != Player.playerState.RUN)
            {
                print("しゃがみ反応");
                Squat();
            }
        }

        //最終的な移動速度の計算
        if (_player.PlayerState == Player.playerState.RUN)
        {
            _move *= _moveSpeed_Run;
        }
        else if(_player.PlayerState == Player.playerState.SQUAT)
        {
            _move *= _moveSpeed_Squat;
        }
        else
        {
            _move *= _moveSpeed;
        }
        this.transform.localPosition += _move;

    }

    //ジャンプ
    public void Jump()
    {

    }

    //スライディング
    public void Sliding()
    {
        _actionFlg = true;
        //当たり判定の回転は、CapsuleColliderに専用の変数がよういされておらず、今回は1,2か使用しないためマジックナンバーを使用
        this.gameObject.GetComponent<CapsuleCollider>().direction = 2;
        StartCoroutine(SlidingEnd());
    }

    IEnumerator SlidingEnd()
    {
        yield return new WaitForSeconds(_slidingtime);
        this.gameObject.GetComponent<CapsuleCollider>().direction = 1;
        _actionFlg = false;
        Squat(true);

    }

    public void Rolling()
    {
        _actionFlg = true;
        this.gameObject.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
        this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
        StartCoroutine(RollingEnd());
    }
    IEnumerator RollingEnd()
    {
        yield return new WaitForSeconds(_rollingtime);
        if (_player.PlayerState != Player.playerState.SQUAT)
        {
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.0f;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        }
        _actionFlg = false;
    }

    //しゃがむの状態を強制的に変化させる
    void Squat(bool flg, Player.playerState state = Player.playerState.IDLE)
    {
        _squatflg = flg;
        //しゃがむ
        if(_squatflg)
        {
            _player.PlayerState = Player.playerState.SQUAT;
            this.gameObject.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
            _squatflg = true;
        }
        //立ち上がる
        else
        {
            _player.PlayerState = state;
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.0f;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
            _squatflg = false;
        }
    }
    //しゃがむの状態を入れ替える
    void Squat()
    {
        //しゃがむ
        if (!_squatflg)
        {
            _player.PlayerState = Player.playerState.SQUAT;
            this.gameObject.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
            _squatflg = true;
        }
        //立ち上がる
        else
        {
            _player.PlayerState = Player.playerState.IDLE;
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.0f;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
            _squatflg = false;
        }
    }
}
