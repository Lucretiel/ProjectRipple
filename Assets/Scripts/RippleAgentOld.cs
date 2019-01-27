using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleAgentOld : MonoBehaviour
{
    public Vector3 movement;
    public float speed = 3f;

    void Start()
    {
        Debug.Log(this.transform.position);
    }

    void Update()
    {
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        speed -= 0.05f;

        if (speed <= 0)
        {
            transform.parent.gameObject.GetComponent<RippleGenerator>().Remove();
        }
    }
}
