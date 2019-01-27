using UnityEngine;
using UnityEngine.EventSystems;

public class Scene : MonoBehaviour, IPointerClickHandler
{
    public GameObject ripple;
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
        GameObject newRipple = Instantiate(ripple, new Vector3(position.x, position.y), Quaternion.identity);
        newRipple.GetComponent<RippleManager>().shader = shader;
    }
}