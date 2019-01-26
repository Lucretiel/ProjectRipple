using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scene : MonoBehaviour, IPointerClickHandler
{
    public GameObject RippleGenerator;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 position = cam.ScreenToWorldPoint(eventData.position);
        var test = Instantiate(RippleGenerator, new Vector3(position.x, position.y), Quaternion.identity);
        //test.transform.localPosition = eventData.position;
    }
}