using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Animator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newX = (this.transform.position.x >= 10f) ? -10f : this.transform.position.x + 0.005f;
        float newY = (this.transform.position.y >= 6f) ? -6f : this.transform.position.y + 0.005f;
        this.transform.position = new Vector3(newX, newY);
    }
}
