using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public void PopEnemy()
    {
        var enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)], m_popPoint.position, m_popPoint.rotation, m_popParent);
        enemy.GetComponent<Enemy.IEnemy>().LootPosition = m_Roots[Random.Range(0, m_Roots.Length)].points;
    }

    public void PopEnemy(int n)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
        }
    }

    public void PopEnemy(int n, float delay)
    {
        StartCoroutine(popEnemy(n, delay));
    }

    IEnumerator popEnemy(int n, float delay)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
            yield return new WaitForSeconds(delay);
        }
    }
}


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
