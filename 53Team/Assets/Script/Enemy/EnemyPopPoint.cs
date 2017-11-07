using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopPoint : MonoBehaviour {

    public GameObject[] m_enemyPrefabs;

    public GameObject m_popParent;

    public void PopEnemy()
    {
        var enemy = Instantiate(m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)], m_popParent.transform);
        //enemy.GetComponent<Enemy.IEnemy>().LootPosition = m_lootPositions[0];
    }

    public void PopEnemy(int n)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
        }
    }

    public void PopEnemy(int n, int delay)
    {
        StartCoroutine(popEnemy(n, delay));
    }

    IEnumerator popEnemy(int n, int delay)
    {
        for (int i = 0; i < n; i++)
        {
            PopEnemy();
            yield return new WaitForSeconds(delay);
        }
    }
}
