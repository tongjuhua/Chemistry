using UnityEngine.Events;
using UnityEngine;

public class PCGrasp : MonoBehaviour {
    [System.Serializable]
    public class EventTrigger {
        public UnityEvent contactBeginEvent = new UnityEvent();
        public UnityEvent graspBeginEvent = new UnityEvent();
        public UnityEvent graspEndEvent = new UnityEvent();
    }

    [SerializeField]
    public EventTrigger graspEventTrigger = new EventTrigger(); 

    private Vector3 selfInitWorldPosition;
    private Vector3 selfScreenPosition;

    private Vector3 mouseInitWorldPosition;
    private Vector3 mouseCurrentWorldPosition;

    private void OnMouseEnter() {
        graspEventTrigger.contactBeginEvent.Invoke();
    }

    private void OnMouseDown() {
        selfInitWorldPosition = transform.position;
        selfScreenPosition = Camera.main.WorldToScreenPoint(selfInitWorldPosition);

        mouseInitWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                selfScreenPosition.z
                ));

        graspEventTrigger.graspBeginEvent.Invoke();
    }

    private void OnMouseDrag() {
        mouseCurrentWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                selfScreenPosition.z
                ));

        transform.position = mouseCurrentWorldPosition - mouseInitWorldPosition + selfInitWorldPosition;
    }

    private void OnMouseUp() {
        graspEventTrigger.graspEndEvent.Invoke();
    }
}
