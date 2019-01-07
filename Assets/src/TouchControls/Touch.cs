using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour
{

    public LocationEvent OnTapLocation;

    public LocationEvent OnStartHold;
    public UnityEvent OnStopHold;
    float holdDuration = 0f;
    bool holding = false;
    bool TouchedButtonSinceLastFrame = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            var touch = Input.touches[0];
            if(holdDuration > .5f)
            {
                if(holding == false)
                {
                    holding = true;
                    OnStartHold.Invoke(touch.position);
                }
            }
            if (holdDuration < .5f)
            {
                var pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touch.position;
                var list = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, list);
                if (list.Count > 0)
                    TouchedButtonSinceLastFrame = true;
                if (touch.phase == TouchPhase.Ended &!TouchedButtonSinceLastFrame)
                    OnTapLocation.Invoke(Camera.main.ScreenToWorldPoint(touch.position));
            } 
        }
        else
        {
            if (holdDuration > 0f)
                OnStopHold.Invoke();
            holdDuration = 0f;
            TouchedButtonSinceLastFrame = false;
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            var position = Input.mousePosition;
            var pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = position;
            var list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, list);
            if (list.Count == 0)
                OnTapLocation.Invoke(Camera.main.ScreenToWorldPoint(position));
        }
#endif
    }

    [System.Serializable]
    public class LocationEvent : UnityEvent<Vector2> { }
}
