using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scene : MonoBehaviour, IPointerClickHandler
{
    public GameObject RippleGenerator;
    private Camera cam;
    private Material shader;

    void Start()
    {
        cam = Camera.main;
        shader = new Material(Shader.Find("Sprites/Default")) { color = Color.blue };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 position = cam.ScreenToWorldPoint(eventData.position);
        GameObject ripple = Instantiate(RippleGenerator, new Vector3(position.x, position.y), Quaternion.identity);
        ripple.GetComponent<RippleGenerator>().shader = shader;
    }
}