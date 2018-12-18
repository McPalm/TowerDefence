using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Attack.Aura
{
    public class HyperUI : MonoBehaviour
    {
        public GameObject UIObject;
        public Text cooldownDisplay;
        public float abilityCooldown;

        Button button;
        float cooldown;
        public bool Available { get { return cooldown < 0f; } }
        bool downtime = true;

        HashSet<HyperSpeed> Clients;

        private void Update()
        {
            if (Available)
            {
                if (Input.GetButtonDown("Boost"))
                    Activate();
                cooldownDisplay.gameObject.SetActive(false);
                button.enabled = true;
            }
            else if (!downtime)
            {
                cooldown -= Time.deltaTime;
                cooldownDisplay.gameObject.SetActive(true);
                cooldownDisplay.text = ((int)cooldown + 1) + "s";
                button.enabled = false;
            }
        }

        private void Start()
        {
            button = UIObject.GetComponentInChildren<Button>();
            UIObject.SetActive(false);
            Clients = new HashSet<HyperSpeed>();
            var waveManager = FindObjectOfType<WaveManagement.WaveManager>();
            waveManager.OnStartDowntime.AddListener(OnDowntime);
            waveManager.OnStartWave.AddListener(OnStartWave);
        }

        void OnStartWave() => downtime = false;
        void OnDowntime() => downtime = true;

        public void Add(HyperSpeed h)
        {
            UIObject.SetActive(true);
            Clients.Add(h);
        }

        public void Remove(HyperSpeed h)
        {
            Clients.Remove(h);
            if (Clients.Count == 0)
                UIObject.SetActive(false);
        }

        public void Activate()
        {
            if(Available)
            {
                foreach (var item in Clients)
                {
                    item.Activate();
                }
                cooldown = abilityCooldown;
            }
        }
    }
}
