using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Enemy;

public class EnemyPopPoint : MonoBehaviour {

    public GameObject[] m_enemyPrefabs;

    public Transform m_popParent;
    public Transform m_popPoint;

    public RootPoints[] m_Roots;
    [System.Serializable]
    public struct RootPoints
    {
        public Transform[] points;
    }

    private int m_squadNum;
    public EnemyGroup m_group;
    [System.Serializable]
    public struct EnemyGroup
    {
        public int group;
        public List<GameObject> squads;
    }

    private readonly float DEF_POP_DELAY = 1.0f;
    private readonly float DEF_POP_WAIT_TIME = 2.0f;

    private void Start()
    {
        // 分隊員数を保存
        m_squadNum = m_group.squads.Count;

        for (int i = 0; i < m_squadNum; i++)
        {
            if (m_group.squads[i] == null) return;

            var e = m_group.squads[i].GetComponent<Enemy_Standard>();
            e.m_group = m_group.group;
        }
    }

    // Enemy生成
    public void PopEnemy()
    {
        var enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)], m_popPoint.position, m_popPoint.rotation, m_popParent);
        enemy.GetComponent<IEnemy>().LootPosition = m_Roots[Random.Range(0, m_Roots.Length)].points;
        enemy.GetComponent<Enemy_Standard>().m_group = m_group.group;
        m_group.squads.Add(enemy);
    }

    // n体Enemy生成
    public void PopEnemy(int n)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
        }
    }

    // n体n秒間隔でEnemy生成
    public void PopEnemy(int n, float delay)
    {
        StartCoroutine(popEnemy(n, delay));
    }

    public void PopEnemy(int n, float delay, float wait)
    {
        StartCoroutine(Wait(wait, () => PopEnemy(n, delay)));
    }

    IEnumerator Wait(float t, System.Action action)
    {
        yield return new WaitForSeconds(t);
        action();
    }

    IEnumerator popEnemy(int n, float delay)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
            yield return new WaitForSeconds(delay);
        }
    }

    // 分隊員が全員しんだか
    public void CheckSquad()
    {
        for (int i = 0; i < m_group.squads.Count; i++)
        {
            var e = m_group.squads[i].GetComponent<Enemy_Standard>();
            if (e._charaPara._hp <= 0)
            {
                m_group.squads.RemoveAt(i);
            }
        }

        if(m_group.squads.Count == 0)
        {
            PopEnemy(m_squadNum, DEF_POP_DELAY, DEF_POP_WAIT_TIME);
        }
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(EnemyPopPoint))]
public class EnemyPopPointEx : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("PopEnemy"))
        {
            Debug.Log("敵生成！！");
            EnemyPopPoint pop = target as EnemyPopPoint;

            pop.PopEnemy();
        }
    }
}
#endif
