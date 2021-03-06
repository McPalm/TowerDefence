using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject Overlay;

        bool selectionStack = false;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetButtonDown("Pause"))
            {
                Paused = true;
            }
            if(Input.GetButtonDown("Cancel"))
            {
                if (selectionStack)
                    selectionStack = false;
                else
                    TogglePause();
            }
        }

        private void Start()
        {
            var selection = FindObjectOfType<Building.Selection.SelectTower>();
            if (selection)
            {
                selection.OnSelect.AddListener(o =>
                {
                    if (o.GetComponent<Building.Upgrades.AUpgrade>())
                        selectionStack = true;
                });
                selection.OnDeselect.AddListener(() => { selectionStack = false; });
            }
        }

        public bool Paused
        {
            set
            {
                Overlay.SetActive(value);
                Time.timeScale = value ? 0f : 1f;
            }
            get
            {
                return Overlay.activeSelf;
            }
        }

        public void TogglePause()
        {
            Paused = !Paused;
        }

        public void ReloadScene()
        {
            Reset.ReloadScene();
        }
    }
}
