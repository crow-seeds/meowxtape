using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    [SerializeField] RectTransform canvas;
    [SerializeField] Camera cam;
    //[SerializeField] AudioSource soundFx;

    Vector2 offset;


    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = GetComponentInParent<EventTrigger>();


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((eventData) => { onDrag(); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.BeginDrag;
        entry2.callback.AddListener((eventData) => { onDragBegin(); });
        trigger.triggers.Add(entry2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onDragBegin()
    {
        //soundFx.PlayOneShot(Resources.Load<AudioClip>("Sounds/paper" + Random.Range(0, 3).ToString()));
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, cam, out localPoint);
        offset = (Vector2)transform.localPosition - localPoint;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }


    public void onDrag()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), Input.mousePosition, cam, out localPoint);
        transform.localPosition = localPoint + offset;
        transform.localPosition = new Vector2(Mathf.Min(Mathf.Max(-800, transform.localPosition.x), 800), Mathf.Min(Mathf.Max(-450, transform.localPosition.y), 450));

    }
}
