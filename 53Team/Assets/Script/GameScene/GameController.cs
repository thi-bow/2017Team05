using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool m_isTutorial = true;

    public EnemyMgr m_enemyMgr;

    [Space(10)]
    public readonly int m_killBorder = 15;

    private IEnumerator Start()
    {
        //Tutorialが終了するまで待機
        yield return new WaitWhile(() => m_isTutorial);
        Debug.Log("チュートリアル終了");
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

    private IEnumerator Clear()
    {
        yield return null;
    }
}