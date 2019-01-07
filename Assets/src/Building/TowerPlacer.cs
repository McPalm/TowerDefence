using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Building
{
    public class TowerPlacer : MonoBehaviour
    {
        public RangeMarker rangeMarker;

        public GameObject prefab;
        SpriteRenderer preview;
        int cost;
        System.Action State;
        float radius = 0f;
        BuildGrid grid;

        // Use this for initialization
        void Start()
        {
            grid = FindObjectOfType<BuildGrid>();
            var o = new GameObject("build swatch");
            preview = o.AddComponent<SpriteRenderer>();
            o.SetActive(false);
            State = Idle;
        }

        public void Select(GameObject prefab, int cost)
        {
            this.cost = cost;
            this.prefab = prefab;
            preview.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            preview.gameObject.SetActive(true);
            State = Placing;
            var turret = prefab.GetComponent<Attack.Turret>();
            var miner = prefab.GetComponent<Attack.MineLayer>();
            if (turret)
                radius = turret.distance;
            else if (miner)
                radius = miner.range;
            else
                radius = 0f;
                
        }

        public void Deselect()
        {
            prefab = null;
            preview.gameObject.SetActive(false);
            State = Idle;
            rangeMarker.Hide();
        }

        // Update is called once per frame
        void Update()
        {
            State();
        }

        void Placing()
        {
            var mp = Input.mousePosition;
            var v3 = Camera.main.ScreenToWorldPoint(mp);
            v3 = new Vector3(Mathf.Round(v3.x + .5f) - .5f, Mathf.Round(v3.y + .5f) - .5f);
            preview.transform.position = v3;
            preview.color = grid.SpaceAvailable(v3) ? Color.white : Color.red;
            if (radius > 0f)
                rangeMarker.Show(v3, radius);

            if (Input.GetMouseButtonDown(0) && preview.color == Color.white)
            {
                if (Score.Wallet.Instance.Money >= cost)
                    Place();
                if(false == Input.GetButton("Queue"))
                    Deselect();
            }
            else if (Input.GetButtonDown("Cancel"))
                Deselect();
        }
        void Idle() { }

        void Place()
        {
            Place(prefab, preview.transform.position, cost);
        }

        static public void Place(GameObject tower, Vector3 position, int price)
        {
            position = new Vector3(Mathf.Round(position.x + .5f) - .5f, Mathf.Round(position.y + .5f) - .5f);
            if (Score.Wallet.Instance.Money >= price && FindObjectOfType<BuildGrid>().SpaceAvailable(position))
            {
                var fab = Instantiate(tower);
                fab.transform.position = position;
                Score.Wallet.Instance.Spend(price);
                fab.AddComponent<Obstruction>();
            }
        }
    }
}
