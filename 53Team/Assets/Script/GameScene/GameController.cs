using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool m_isTutorial = true;
    public static bool _pause = false;
    [SerializeField] private GameObject _pauseUI = null;
    [SerializeField] private Button _reStartButton = null;

    public EnemyMgr m_enemyMgr;

    [Space(10)]
    public readonly int m_killBorder = 15;

    private IEnumerator Start()
    {
        _pause = false;
        //Tutorialが終了するまで待機
        yield return new WaitWhile(() => m_isTutorial);
        Debug.Log("チュートリアル終了");
        ResultScore.KillCount = 0;
        // ゲーム開始に必要な準備の処理

        // 一定数Playerが敵を倒すまで待つ
        yield return new WaitWhile(() => ResultScore.KillCount < m_killBorder);

        // ボス出現処理
        m_enemyMgr.PopBossEnemy();
        // ボスが倒されるまで待つ
        yield return new WaitWhile(() => !m_enemyMgr.IsBossDead());

        // ゲームクリア演出

        yield return Clear();

        // 次のシーンへ
        SceneManagerScript.sceneManager.SceneOut(SceneName.sceneName.Result.ToString());
    }

    public void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            GamePause();
        }
    }

    private IEnumerator Clear()
    {
        yield return null;
    }

    public void GamePause()
    {
        if (!_pause)
        {
            SceneManagerScript.sceneManager.FadeBlack();
            _pause = true;
            _pauseUI.SetActive(true);
            _reStartButton.Select();
        }
        else
        {
            _pauseUI.SetActive(false);
            SceneManagerScript.sceneManager.FadeWhite();
            _pause = false;
        }
    }
}