using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Attack.Projectile
{
    public class InstantLine : MonoBehaviour, IProjectile
    {
        public GameObject prefab;

        public bool stretch = true;
        public bool flipY;

        public void Shoot(GameObject target, System.Action<GameObject> action)
        {
            action(target);
            StartCoroutine(ShowLine(target));
        }

        IEnumerator ShowLine(GameObject target)
        {
            var fab = Instantiate(prefab);
            fab.transform.position = (target.transform.position + transform.position) / 2;
            var distance = (target.transform.position - transform.position).magnitude;
            if(stretch)
                fab.transform.localScale = new Vector3(distance, 1f, 1f);
            fab.transform.right = target.transform.position - transform.position;
            if (flipY && Random.value < .5f)
                fab.GetComponent<SpriteRenderer>().flipY = true;
            yield return new WaitForSeconds(.1f);
            Destroy(fab);
        }
        
    }
}