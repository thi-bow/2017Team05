using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        AttackTutorial,
        PartsAddTutorial,
        SecondAttackTutorial,
        BodyPartsAddTutorial,
        PargeTutorial,
        TakeWepon,
        End,
    }
    public TutorialState _tutorialState = TutorialState.AttackTutorial;
    public static bool tutorialMove = false;    //Tutorialで動作を確認中はtrueになる
    [SerializeField] private Player _player = null;
    [SerializeField] private PlayerMove _playerMove = null;
    [SerializeField] private PlayerSkyMove _playerSkyMove = null;
    [SerializeField] private GameObject _tutorialUI = null;
    private TutorialTextManager _tutorialTextManager = null;
    private TutorialClearChecker _tutorialClearChecker = null;

    private bool attackCheck = false;
    private bool partsAddCheck = false;
    private bool secondAttackCheck = false;
    private bool bodyPartsAddCheck = false;
    private bool fullPargeCheck = false;

    private bool attackTutorial = false;
    private bool partsRobTutorial = false;
    private bool pargeTutorial = false;
    private bool takeWepon = false;

    // Use this for initialization
    void Start () {

        //_player.enabled = false;
        //_playerMove.enabled = false;
        //_playerSkyMove.enabled = false;
        _tutorialTextManager = this.GetComponent<TutorialTextManager>();
        _tutorialClearChecker = this.GetComponent<TutorialClearChecker>();
        StartCoroutine(_tutorialTextManager.TextWrite((int)_tutorialState));
    }
	
	// Update is called once per frame
	void Update () {
        if (!tutorialMove) return;
        switch (_tutorialState)
        {
            case TutorialState.AttackTutorial:
                if (!attackCheck && _tutorialClearChecker.AttackCheck())
                {
                    attackCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    _tutorialState = TutorialState.PartsAddTutorial;
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.AttackTutorial + 1));
                }
                break;
            case TutorialState.PartsAddTutorial:
                if(!partsAddCheck && _tutorialClearChecker.PartsAddCheck())
                {
                    partsAddCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    _tutorialState = TutorialState.SecondAttackTutorial;
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.PartsAddTutorial + 1));
                }
                break;
            case TutorialState.SecondAttackTutorial:
                if (!secondAttackCheck && _tutorialClearChecker.SecontEnemyAttack())
                {
                    secondAttackCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    _tutorialState = TutorialState.BodyPartsAddTutorial;
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.SecondAttackTutorial + 1));
                }
                break;
            case TutorialState.BodyPartsAddTutorial:
                if (!bodyPartsAddCheck && _tutorialClearChecker.BodyPartsAddCheck())
                {
                    bodyPartsAddCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    _tutorialState = TutorialState.PargeTutorial;
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.BodyPartsAddTutorial + 1));
                }
                break;
            case TutorialState.PargeTutorial:
                if (!fullPargeCheck && _tutorialClearChecker.FullPargeCheck())
                {
                    fullPargeCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    _tutorialState = TutorialState.TakeWepon;
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.PargeTutorial + 1, TutorialEnd));
                }
                break;
            case TutorialState.TakeWepon:
                break;
            default:
                break;
        }

    }

    public void TutorialEnd()
    {
        GameController.m_isTutorial = false;
        gameObject.SetActive(false);
        _tutorialUI.SetActive(false);
    }
}
