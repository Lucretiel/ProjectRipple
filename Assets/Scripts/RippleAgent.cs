using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleAgent : MonoBehaviour
{
    public Vector3 movement;

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(movement * Time.deltaTime, Space.World);
    }
}
