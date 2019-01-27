using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleManager : MonoBehaviour
{
    public Material shader;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.material = shader;
    }

    void Update()
    {
        RippleAgent origin = this.GetComponentInChildren<RippleAgent>();
        RippleAgent traversal = origin;
        int count = 0;
        int total = this.GetComponentsInChildren<RippleAgent>().Length + 1; //.transform.childCount + 1;
        lineRenderer.positionCount = total;

        do
        {
            lineRenderer.SetPosition(count++, traversal.transform.position);
            traversal = traversal.leftNeighbor;
        }
        while (traversal != origin);

        // close the loop
        lineRenderer.SetPosition(count, origin.transform.position);
    }

    public void Remove()
    {
        Object.Destroy(gameObject);
    }
}
