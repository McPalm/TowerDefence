using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Attack
{
    public class AIUI : MonoBehaviour
    {
        public GameObject UiRoot;

        public Text priorityText;

        public Button First;
        public Button Last;
        public Button Strong;
        public Button Weak;
        public Button Random;
        public Button Lock;

        public Color LockColor;
        public Color UnLockColor;

        Turret selected;

        private void Start()
        {
            var select = FindObjectOfType<Building.Selection.SelectTower>();
            select.OnSelect.AddListener(OnSelect);
            select.OnDeselect.AddListener(Hide);
            Hide();
            First.onClick.AddListener(SetFirst);
            Last.onClick.AddListener(SetLast);
            Strong.onClick.AddListener(SetStrong);
            Weak.onClick.AddListener(SetWeak);
            Random.onClick.AddListener(SetRandom);
            Lock.onClick.AddListener(ToggleLock);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
                Hide();
        }

        public void OnSelect(GameObject turret)
        {
            var s = turret.GetComponent<Turret>();
            if (s)
            {
                selected = s;
                if (selected)
                    Show();
            }
            else
                Hide();
        }


        void Show()
        {
            FormatThing();
            UiRoot.SetActive(true);
        }

        void FormatThing()
        {
            //   priorityText.text = selected.targetPriority.ToString() + (selected.LockTarget ? "(lock)" : "");
            First.interactable = selected.targetPriority != Turret.TargetPriority.first;
            Last.interactable = selected.targetPriority != Turret.TargetPriority.last;
            Strong.interactable = selected.targetPriority != Turret.TargetPriority.strongest;
            Weak.interactable = selected.targetPriority != Turret.TargetPriority.weakest;
            Random.interactable = selected.targetPriority != Turret.TargetPriority.random;
            Lock.GetComponent<Image>().color = selected.LockTarget ? LockColor : UnLockColor;
        }

        void Hide()
        {
            UiRoot.SetActive(false);
        }

        public void SetFirst()
        {
            if(selected)
                selected.targetPriority = Turret.TargetPriority.first;
            FormatThing();
        }

        public void SetLast()
        {
            if (selected)
                selected.targetPriority = Turret.TargetPriority.last;
            FormatThing();
        }

        public void SetStrong()
        {
            if (selected)
                selected.targetPriority = Turret.TargetPriority.strongest;
            FormatThing();
        }

        public void SetWeak()
        {
            if (selected)
                selected.targetPriority = Turret.TargetPriority.weakest;
            FormatThing();
        }

        public void SetRandom()
        {
            if (selected)
                selected.targetPriority = Turret.TargetPriority.random;
            FormatThing();
        }

        public void ToggleLock()
        {
            if (selected)
                selected.LockTarget = !selected.LockTarget;
            FormatThing();
        }
    }
}