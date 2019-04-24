using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Progression
{
    public class ChallengeWindow : MonoBehaviour
    {
        Tile current;
        public GameObject Window;
        public List<Image> EnemyIcons;

        public void Challenge(Tile t)
        {
            Window.SetActive(true);
            current = t;
            var sprites = new HashSet<Sprite>();
            foreach (var wave in t.Army.waves)
            {
                foreach (var unit in wave.units)
                {
                    var sprite = unit.enemy.GetComponent<SpriteRenderer>();
                    if (sprite)
                        sprites.Add(sprite.sprite);
                }
            }
            var enumerator = sprites.GetEnumerator();

            for (int i = 0; i < EnemyIcons.Count; i++)
            {
                if(enumerator.MoveNext())
                {
                    EnemyIcons[i].gameObject.SetActive(true);
                    EnemyIcons[i].sprite = enumerator.Current;
                }
                else
                {
                    EnemyIcons[i].gameObject.SetActive(false);
                }
            }

        }

        public void StartChallenge()
        {

        }
    }
}
