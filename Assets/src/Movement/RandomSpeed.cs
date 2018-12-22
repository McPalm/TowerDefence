using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class RandomSpeed : MonoBehaviour
    {
        public float frequency = 1f;
        public float MinSpeed = 1f;
        public float MaxSpeed = 3f;


        public Sprite slowSprite;
        public Sprite fastSprite;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(SpeedRoshambo());
        }

        IEnumerator SpeedRoshambo()
        {
            yield return null;
            var mobile = GetComponent<Mobile>();

            while (true)
            {
                var random = Random.value;
                var random2 = Random.value;
                random = Mathf.Abs(random - .5f) > Mathf.Abs(random2 - .5f) ? random : random2;
                mobile.speed = MinSpeed + (MaxSpeed - MinSpeed) * random;
                
                if(slowSprite && fastSprite)
                {
                    GetComponent<SpriteRenderer>().sprite = random < .5f ? slowSprite : fastSprite;
                }

                yield return new WaitForSeconds(frequency * Random.value + frequency * .5f);
            }
        }
    }
}
