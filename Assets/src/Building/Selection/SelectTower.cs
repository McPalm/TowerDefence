using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Building.Selection
{
    public class SelectTower : MonoBehaviour
    {

        public GameObjectEvent OnSelect;
        // invoked on an empty click
        public UnityEvent OnDeselect;

        /// <summary>
        /// True if the mouse is hovering over an UI component.
        /// </summary>
        static public bool Blocked
        {
            get
            {
                return EventSystem.current.IsPointerOverGameObject();
            }
        }

        /// <summary>
        /// The location of the mouse in the world space (as opposed to the UI space)
        /// </summary>
        static public Vector3 MousePosition
        {
            get
            {
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        System.Action State;

        void Start()
        {
            State = Idle;
        }

        private void Update()
        {
            State();
        }

        void Idle()
        {
            if (Blocked)
                return;
            if(Input.GetMouseButtonDown(0))
            {
                var o = FindObjectOfType<BuildGrid>().ObjectAt(MousePosition);
                if (o)
                    OnSelect.Invoke(o);
                else
                    OnDeselect.Invoke();

            }
        }
    }
}