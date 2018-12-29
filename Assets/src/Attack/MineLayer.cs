using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Attack
{
    public class MineLayer : MonoBehaviour {

        public GameObject prefab;
        

        public int minesPerWave = 6;
        public float range = 2.1f;
        public int damage;
        public float explosionRadius;
        public int maxTargets = 1;

        public float stunDuration = 0f;
        public float slowFactor = 1f;


        float cooldown = 2f;
        float mineLifeTime = 500f;

        int minesPlaced = 0;
        Animator animator;

        HashSet<Mine> mines;

        private void Start()
        {
            mines = new HashSet<Mine>();
            var waveManager = FindObjectOfType<WaveManagement.WaveManager>();
            waveManager.OnStartWave.AddListener(StartWave);
            waveManager.OnStartDowntime.AddListener( () => minesPlaced = 99999999);
            animator = GetComponent<Animator>();
        }

        void StartWave()
        {
            minesPlaced = 0;
            cooldown = .5f;
            foreach (var mine in mines)
                if(mine)
                    Destroy(mine.gameObject);
            mines.Clear();
        }

        // Update is called once per frame
        void Update()
        {
            if (minesPlaced < minesPerWave)
            {
                if (cooldown < 0f)
                {
                    cooldown += minesPerWave > 60 ? .3f : 1.2f;
                    PlaceMine();
                }
                cooldown -= Time.deltaTime;
            }
        }

        void PlaceMine()
        {
            var fab = Instantiate(prefab);
            var mine = fab.GetComponent<Mine>();
            mine.damage = damage;
            mine.radius = explosionRadius;
            mine.lifetime = mineLifeTime;
            mine.transform.position = RandomSpawnPoint() + Scatter();
            mine.stunDuration = stunDuration;
            mines.Add(mine);
            mine.maxTargets = maxTargets;
            minesPlaced++;
            if (animator)
                animator.SetTrigger("Attack");
            GetComponent<SpriteRenderer>().flipX = mine.transform.position.x < transform.position.x;
        }

        List<Transform> SpawnPoints;

        public void RefreshSpawnPoints()
        {
            var meshManager = FindObjectOfType<Movement.MeshManager>();
            SpawnPoints = new List<Transform>();
            foreach (var mesh in meshManager.waypointMeshes)
            {
                SpawnPoints.AddRange(
                    mesh.trackNodes
                    .Where(o => (o.transform.position - transform.position).sqrMagnitude <= range * range)
                    .Select(o => o.transform)
                    );
            }
        }

        Vector3 RandomSpawnPoint()
        {
            if (SpawnPoints == null)
                RefreshSpawnPoints();

            if(SpawnPoints.Count > 0)
            {
                return SpawnPoints[DrawNumber()].position;
            }
            return new Vector3(-50, -50);
        }

        Vector3 Scatter()
        {
            return new Vector3(-.5f + Random.value, -.5f + Random.value, 0f) * .8f;
        }

        List<int> numbers;
        int DrawNumber()
        {
            if(numbers == null || numbers.Count == 0)
            {
                numbers = new List<int>();
                for (int i = 0; i < SpawnPoints.Count; i++)
                {
                    numbers.Add(i);
                }
            }
            int rng = Random.Range(0, numbers.Count);
            int ret = numbers[rng];
            numbers.RemoveAt(rng);
            return ret;
        }
    }
}