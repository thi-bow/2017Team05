using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player _player = null;
    [SerializeField]
    private GameObject _mainCamera = null;

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
    private bool _runFlg = false;
    private bool _squatflg = false;
    public Vector3 _move = new Vector3(0.0f, 0.0f, 0.0f);
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
    private bool _jumpFlg = false;

    //--------------当たり判定の大きさ
    [SerializeField]
    private float _smallColliderHeight = 1.0f;
    [SerializeField]
    private float _smallColliderPositionY = -0.5f;

    private bool _actionFlg = false;

    #endregion
    

    // Use this for initialization
    void Start()
    {
        _player = this.gameObject.GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        /*if(_rollingFlag && Input.GetButtonDown("Jump"))
        {
            Jump();
            return;
        }*/

        //走るかどうか
        if (Input.GetButtonDown("Run") && !_player.AttackCheck)
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

        if (thirdPersonCamera != null)
        {

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
        }

        if(_player.PlayerState == Player.playerState.RUN)
        {
            RunMove();
        }

        else if(_player.PlayerState == Player.playerState.SQUAT ||
                _player.PlayerState == Player.playerState.MOVE)
        {
            WalkMove();
        }
        

    }

    //歩いてるときにできる行動
    void WalkMove()
    {
        
        //走っていないときにしゃがむボタンを押すとしゃがむ
        if (Input.GetKeyDown(KeyCode.J))
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
        this.transform.localPosition += _move;

    }

    //走っているときにできる行動
    void RunMove()
    {
        if (!_actionFlg)
        {
            //移動処理
            _move *= _moveSpeed_Run;
        }
        this.transform.localPosition += _move;

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

    //ジャンプ
    public void Jump()
    {
        if(_jumpFlg == true)
        {
            _player.PlayerState = Player.playerState.SKYMOVE;
            _jumpFlg = false;
        }
        if(_slidingFlg)
        {
            SlidingCancel(true);
        }

        _jumpFlg = true;
    }

    #region スライディング
    //スライディング
    public void Sliding()
    {
        _runFlg = false;
        _slidingFlg = true;
        _actionFlg = true;
        //当たり判定の回転は、CapsuleColliderに専用の変数がよういされておらず、今回は1,2か使用しないためマジックナンバーを使用
        this.gameObject.GetComponent<CapsuleCollider>().direction = 2;
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
        this.gameObject.GetComponent<CapsuleCollider>().direction = 1;
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
        this.gameObject.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
        this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
        StartCoroutine(AvoidEnd());
    }
    IEnumerator AvoidEnd()
    {
        yield return new WaitForSeconds(_rollingtime);
        if (_player.PlayerState != Player.playerState.SQUAT)
        {
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.0f;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
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
            this.gameObject.GetComponent<CapsuleCollider>().height = _smallColliderHeight;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, _smallColliderPositionY, 0);
        }
        //立ち上がる
        else
        {
            _player.PlayerState = state;
            this.gameObject.GetComponent<CapsuleCollider>().height = 2.0f;
            this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
        }
    }
    //しゃがむの状態を入れ替える
    void Squat()
    {
        Squat(!_squatflg);
    }
    #endregion

}
