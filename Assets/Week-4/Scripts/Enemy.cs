using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Enemy : MonoBehaviour
    {
        public int health = 10;

        [SerializeField] private Player target;

        public void Damage(int amt)
        {
            health -= amt;
        }

        [ContextMenu("Attack")]
        void Attack()
        {
            target.Damage(3);
        }
    }
}
