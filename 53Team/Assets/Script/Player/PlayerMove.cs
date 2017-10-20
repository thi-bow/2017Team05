using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector3 check; 

    [SerializeField] private Player _player = null;
    private GameObject _parent = null;
    private PlayerSkyMove _playerSkyMove = null;
    private GameObject _mainCamera = null;
    private Rigidbody _myRB = null;

    // インスペクターで主観カメラを紐づける
    [SerializeField]private GameObject firstPersonCamera;
    // インスペクターで第三者視点カメラを紐づける
    [SerializeField]private GameObject thirdPersonCamera;

    #region 移動に関する変数
    [Header("----------------移動速度---------------------")]
    [SerializeField]
    private float _moveSpeed = 1.0f;
    [SerializeField]
    private float _moveSpeed_Run = 2.0f;
    [SerializeField]
    private float _moveSpeed_Squat = 0.5f;
    [SerializeField]
    private float _moveSpeed_Jump = 0.5f;
    private bool _runFlg = false;
    private bool _squatflg = false;
    private Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
    #endregion

    #region 特殊移動に関する変数
    [Header("--------------特殊行動の詳細------------------")]
    //--------------スライディング
    [SerializeField]
    private float _slidingtime = 1.0f;
    private bool _slidingFlg = false;

    //--------------回転回避
    [SerializeField]
    private float _rollingtime = 0.3f;
    private bool _rollingFlag = false;

    //--------------ジャンプ
    [SerializeField] private float _jumpPower = 5.0f;
    private bool _jumpFlg = false;

    //--------------当たり判定の大きさ
    [SerializeField]
    private float _smallColliderHeight = 1.0f;
    [SerializeField]
    private float _smallColliderPositionY = -0.5f;

    private bool _actionFlg = false;
    #endregion

    [Header("--------------Rayの長さ------------------")]
    [SerializeField] private float rayLength = 5.0f;


    // Use this for initialization
    void Start()
    {
        _parent = _player.gameObject;
        _myRB = _parent.GetComponent<Rigidbody>();
        _playerSkyMove = this.gameObject.GetComponent<PlayerSkyMove>();
        _mainCamera = _player._mainCamera;
    }
    
    public void Move()
    {
        //中に浮いてたら下に移動させる(ジャンプ中はできない)
        if (!_jumpFlg && Mathf.Abs(_move.x) + Mathf.Abs(_move.z) > 0)
        {
            RayCheck();
        }

        //ジャンプ中はゆっくり移動以外の移動に関する動作はできない
        if (_jumpFlg)
        {
            _myRB.velocity *= 0.98f;
            var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            var _jumpMove = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
            _jumpMove *= _moveSpeed_Jump;

            if (Mathf.Abs(_jumpMove.x) + Mathf.Abs(_jumpMove.z) > 0)
            {
                _myRB.velocity += new Vector3(_jumpMove.x, 0, _jumpMove.z);
            }
        }


        //走るかどうか
        if (Input.GetButtonDown("Run") && !_player.AttackCheck && !_jumpFlg)
        {
            _runFlg = true;
            //スラディングをしていたら、キャンセル
            if (_slidingFlg)
            {
                SlidingCancel(true, Player.playerState.RUN);
            }
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
        

        //スライディング,回避中なら移動の入力を受け付けない
        if (!_actionFlg)
        {
            //-----------今は中の処理は一緒だが、後に変わるかも可能性が高いため現段階からif文を分けておく-----------//
            //走ってはいなく、尚且つしゃがんでいなかったら歩く
            if (_player.PlayerState != Player.playerState.SQUAT && !_runFlg)
            {
                _player.PlayerState = Player.playerState.MOVE;
            }

            //方向キーを離す、下に倒すをしたら走るのを終了する
            if (Input.GetAxis("Vertical") <= 0 && _runFlg)
            {
                _player.PlayerState = Player.playerState.MOVE;
            }

            var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            _move = _moveForward * Input.GetAxis("Vertical") + _mainCamera.transform.right * Input.GetAxis("Horizontal");
        }

        // キャラクターの向きを進行方向に
        if (_move != Vector3.zero && thirdPersonCamera.activeInHierarchy)
        {
            transform.rotation = Quaternion.LookRotation(_move);
        }

        // スペースキーでカメラを切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ↓現在のactive状態から反転 
            firstPersonCamera.SetActive(!firstPersonCamera.activeInHierarchy);
            thirdPersonCamera.SetActive(!thirdPersonCamera.activeInHierarchy);
        }


        if(!_jumpFlg)
        {
            if (_player.PlayerState == Player.playerState.RUN)
            {
                RunMove();
            }

            else if (_player.PlayerState == Player.playerState.SQUAT ||
                    _player.PlayerState == Player.playerState.MOVE)
            {
                WalkMove();
            }
        }

        //ジャンプ
        if (!_rollingFlag && Input.GetButtonDown("Jump"))
        {
            Jump(_move, _jumpPower);
            print("ジャンプ");
        }

        check = _myRB.velocity;

    }

    //歩いてるときにできる行動
    void WalkMove()
    {
        
        //走っていないときにしゃがむボタンを押すとしゃがむ
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("しゃがみ反応");
            Squat();
        }

        //回避が入力されたときの処理
        if (Input.GetKeyDown(KeyCode.T) && !_actionFlg)
        {
            print("回転回避");
            //回転実行
            Avoid();

            //スティックを入力していないか確認し、入力がされていなかったら前方方向に回転
            if (Input.GetAxis("Vertical") == 0.0f && Input.GetAxis("Horizontal") == 0.0f)
            {
                var _moveForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
                _move = _moveForward * 1.0f;
            }
            //回転時の移動速度を走っているときと同じにする
            _move *= _moveSpeed_Run;
        }

        if (!_actionFlg)
        {
            //移動処理
            if (_player.PlayerState == Player.playerState.SQUAT)
            {
                _move *= _moveSpeed_Squat;
            }
            else
            {
                _move *= _moveSpeed;
            }
        }
        //移動スピードが変更されている場合はここで変更を対応させる
        _move *= _player.Speed;
        //this.transform.localPosition += _move;
        _myRB.MovePosition(_move + _parent.transform.position);

    }

    //走っているときにできる行動
    void RunMove()
    {
        if (!_actionFlg)
        {
            //移動処理
            _move *= _moveSpeed_Run;
        }
        //移動スピードが変更されている場合はここで変更を対応させる
        _move *= _player.Speed;
        //this.transform.localPosition += _move;
        _myRB.MovePosition(_move + _parent.transform.position);

        //スライディングが入力されたとき
        if (Input.GetKeyDown(KeyCode.F))
        {
            float _moveCheck = 0.0f;
            _moveCheck = _move.x + _move.z;

            //スティックを一定数傾けていないと反応しない
            if ((_moveCheck >= 0.5f || _moveCheck <= -0.5f))
            {
                print("スライディング反応");
                Sliding();
            }
        }

    }

    #region ジャンプ
    public void Jump(Vector3 moveSpeed, float jumpPower)
    {
        /*//ブースターを所持したいなかったら、ジャンプ中は何もできない
        if(_jumpFlg == true && _player.BoosterArmorList.Count <= 0)
        {
            return;
        }*/
        if (_jumpFlg == true && _playerSkyMove.BoostGage > 0)
        {
            _player.PlayerState = Player.playerState.SKYMOVE;
            _jumpFlg = false;
            _playerSkyMove.UseBoost = true;
            _myRB.useGravity = false;
            _myRB.velocity = new Vector3(0, 0, 0);
            return;
        }
        if(_slidingFlg)
        {
            SlidingCancel(true);
        }

        if (!_jumpFlg)
        {
            _myRB.velocity = new Vector3(moveSpeed.x * 60, jumpPower, moveSpeed.z * 60);
            _myRB.useGravity = true;

            _jumpFlg = true;
        }
    }

    public bool JumpFlg
    {
        get { return _jumpFlg; }
        set { _jumpFlg = value; }
    }
    #endregion

    #region スライディング
    //スライディング
    public void Sliding()
    {
        _runFlg = false;
        _slidingFlg = true;
        _actionFlg = true;
        //当たり判定の回転は、CapsuleColliderに専用の変数がよういされておらず、今回は1,2か使用しないためマジックナンバーを使用
        _parent.GetComponent<CapsuleCollider>().direction = 2;
        StartCoroutine(SlidingEnd());
    }

    IEnumerator SlidingEnd()
    {
        yield return new WaitForSeconds(_slidingtime);
        if (!_slidingFlg)
        {
            yield break;
        }
        SlidingCancel();

    }

    void SlidingCancel(bool endrun = false, Player.playerState endState = Player.playerState.IDLE)
    {
        _parent.GetComponent<CapsuleCollider>().direction = 1;
        _actionFlg = false;
        _slidingFlg = false;
        print("スライディング終了");
        if (!endrun)
        {
            Squat(true);
        }
        else
        {
            _player.PlayerState = endState;
        }
    }

    public void SlidingCheckChancel()
    {
        if(_slidingFlg)
        {
            SlidingCancel(true);
        }
    }
    #endregion

    #region 回避
    //回避アクション
    public void Avoid()
    {
        _actionFlg = true;
        _rollingFlag = true;
        _parent.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
        _parent.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
        StartCoroutine(AvoidEnd());
    }
    IEnumerator AvoidEnd()
    {
        yield return new WaitForSeconds(_rollingtime);
        if (_player.PlayerState != Player.playerState.SQUAT)
        {
            _parent.GetComponent<CapsuleCollider>().height = 2.0f;
            _parent.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        }
        _rollingFlag = false;
        _actionFlg = false;
    }
    #endregion

    #region しゃがむ
    //しゃがむの状態を強制的に変化させる
    void Squat(bool flg, Player.playerState state = Player.playerState.IDLE)
    {
        _squatflg = flg;
        //しゃがむ
        if (_squatflg)
        {
            _player.PlayerState = Player.playerState.SQUAT;
            _parent.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
            _parent.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
        }
        //立ち上がる
        else
        {
            _player.PlayerState = state;
            _parent.GetComponent<CapsuleCollider>().height = 2.0f;
            _parent.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
        }
    }
    //しゃがむの状態を入れ替える
    void Squat()
    {
        Squat(!_squatflg);
    }
    #endregion

    //Ray確認
    #region Ray
    void RayCheck()
    {
        if (_player.PlayerState == Player.playerState.RUN)
        {
            RayAction(rayLength * 4);
        }
        else
        {
            RayAction(rayLength);
        }
    }
    void RayAction(float length)
    {
        RaycastHit _hit;
        Ray _ray;
        Vector3 _pos = Vector3.zero;
        _ray = new Ray(transform.position + new Vector3(0, 1, 0), -this.transform.up);

        if (Physics.Raycast(_ray, out _hit, length))
        {
            Debug.DrawLine(_ray.origin, _hit.point, Color.red);
        }

        if (_hit.collider != null && _hit.collider.tag != "Player")
        {
            _myRB.useGravity = false;
            //print(_hit.point.y);
            _parent.transform.position = new Vector3(_parent.transform.position.x, _hit.point.y, _parent.transform.position.z);
        }
        else
        {
            _myRB.useGravity = true;
            Jump(_move, 0.0f);
            //_parent.transform.position += new Vector3(0, -0.1f, 0);
        }
    }
    #endregion

    #region MovePara
    public Vector3 MovePara
    {
        get { return _move; }
    }
    #endregion

}
