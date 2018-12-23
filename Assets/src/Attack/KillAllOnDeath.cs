using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public class KillAllOnDeath : MonoBehaviour, Enemy.IOnKilled
    {
        public void OnKilled()
        {
            var me = GetComponent<Enemy>();
            Enemy[] copy = new Enemy[Enemy.Enemies.Count];
            Enemy.Enemies.CopyTo(copy);
            foreach (var enemy in copy)
            {
                if (enemy != me)
                    Destroy(enemy.gameObject);
            }
        }
    }
}
