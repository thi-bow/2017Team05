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

    #region 攻撃
    [SerializeField] private Image _skillGageImage = null;
    private float _skillGage = 0.0f;
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
        if(Input.GetKeyDown(KeyCode.O))
        {
            SkillGageAdd(10);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SkillGageSub(10);
        }
        else if(Input.GetKeyDown(KeyCode.L))
        {
            SkillGageReset();
        }
    }

    public void SkillGageAdd(float addNumber = 1.0f)
    {
        _skillGage += addNumber;
        if(_skillGage >= 100.0f)
        {
            _skillGage = 100.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    public void SkillGageSub(float subNumber = 25.0f)
    {
        _skillGage -= subNumber;
        if (_skillGage <= 0.0f)
        {
            _skillGage = 0.0f;
        }
        _skillGageImage.fillAmount = _skillGage / 100.0f;
    }

    public void SkillGageReset()
    {
        _skillGage = 0.0f;
        _skillGageImage.fillAmount = 0.0f;
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
