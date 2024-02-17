using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int health = 10;

        public void Damage(int amt)
        {
            health -= amt;
        }

        public int GetHealth()
        {
            return health;
        }

        private Enemy FindNewTarget()
        {
            // return GameObject.FindAnyObjectByType<Enemy>(); // One enemy only (first in list)
            // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            // int randomIndex = Random.Range(0, enemies.Length);
            // return enemies[randomIndex];

            GameObject enemyObj = GameObject.Find("Enemy");
            Enemy enemyComp = enemyObj.GetComponent<Enemy>();
            return enemyComp;

        }

        [ContextMenu("Attack")]
        void Attack()
        {
            Enemy target = FindNewTarget();
            target.Damage(10);
        }
    }
}
