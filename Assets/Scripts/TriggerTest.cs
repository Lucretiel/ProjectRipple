using UnityEngine;

// GOOOOOOOOAAAAAAAALLLLLLLLLLLLLLL!
public class TriggerTest : MonoBehaviour {
    public float xOffset;
    public float yOffset;

    void Update() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x += xOffset;
        mousePosition.y += yOffset;
        var target = Camera.main.ScreenToWorldPoint(mousePosition);
        target.z = transform.position.z;
        transform.position = target;
    }
}
