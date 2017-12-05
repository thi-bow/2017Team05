using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState
    {
        PartsAddTutorial,
        AttackTutorial,
        PartsRobTutorial,
        PargeTutorial,
        TakeWepon,
        End,
    }
    public TutorialState _tutorialState = TutorialState.PartsAddTutorial;
    public static bool tutorialMove = false;    //Tutorialで動作を確認中はtrueになる
    [SerializeField] private Player _player = null;
    [SerializeField] private PlayerMove _playerMove = null;
    [SerializeField] private PlayerSkyMove _playerSkyMove = null;
    [SerializeField] private GameObject _tutorialUI = null;
    private TutorialTextManager _tutorialTextManager = null;

    private bool partsAddCheck = false;
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
        StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.PartsAddTutorial));
    }
	
	// Update is called once per frame
	void Update () {
        if (!tutorialMove) return;
        switch (_tutorialState)
        {
            case TutorialState.PartsAddTutorial:
                if(Input.GetAxis("crossX") > 0 && !partsAddCheck)
                {
                    partsAddCheck = true;
                    tutorialMove = false;   //テキストを表示している間は、Tutorialのクリア判定を取らないようにする
                    StartCoroutine(_tutorialTextManager.TextWrite((int)TutorialState.PartsAddTutorial + 1));
                    _tutorialState = TutorialState.AttackTutorial;
                }
                break;
            case TutorialState.AttackTutorial:
                break;
            case TutorialState.PartsRobTutorial:
                break;
            case TutorialState.PargeTutorial:
                break;
            case TutorialState.TakeWepon:
                break;
            case TutorialState.End:
                break;
            default:
                break;
        }

    }

    public void TutorialEnd()
    {
        GameController.m_isTutorial = false;
        this.gameObject.SetActive(false);
        _tutorialUI.SetActive(false);
    }
}
