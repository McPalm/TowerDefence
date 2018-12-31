using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;

namespace Movement
{
    public class DropFromTheSky : MonoBehaviour
    {
        public float spawnDuration;

        // Use this for initialization
        void Start()
        {
            
            var mobile = GetComponent<Mobile>();
            var mesh = FindObjectOfType<MeshManager>().GetMesh();

            float spawnLocation = (.1f + .6f * Random.value * Random.value) * mesh.length;
            mobile.location = spawnLocation;
            mobile.mesh = mesh;
            var pos = mesh.DistanceToPosition(spawnLocation);
            StartCoroutine(SpawnInRoutine(pos));
        }

        IEnumerator SpawnInRoutine(Vector2 destination)
        {
            var dummy = new GameObject("Dummy");
            dummy.AddComponent<Enemy>();
            dummy.transform.position = new Vector3(-30, -30, 0);
            Vector2 origin = destination + new Vector2(0f, 13f);

            var sprite = GetComponent<SpriteRenderer>();
            sprite.flipY = true;
            transform.position = origin;
            var origLayer = sprite.sortingLayerID;
            sprite.sortingLayerID = SortingLayer.NameToID("Sky");

            var enemy = GetComponent<Attack.Enemy>();
            var mobile = GetComponent<Mobile>();
            var collider = GetComponent<Collider2D>();
            
            collider.enabled = false;
            enemy.enabled = false;
            mobile.enabled = false;

            yield return new WaitForFixedUpdate();
            for (float t = 0; t < 1f; t += Time.fixedDeltaTime / spawnDuration)
            {
                transform.position = Vector2.Lerp(origin, destination, t);

                yield return new WaitForFixedUpdate();
            }
            sprite.flipY = false;
            sprite.sortingLayerID = origLayer;

            yield return new WaitForSeconds(.2f + Random.value * .35f);

            transform.position = destination;
            mobile.enabled = true;
            enemy.enabled = true;
            collider.enabled = true;
            Destroy(dummy);
            
        }
    }
}