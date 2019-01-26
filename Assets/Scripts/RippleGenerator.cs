using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleGenerator : MonoBehaviour
{
    public GameObject RippleResource;
    GameObject[] RippleList = new GameObject[10];
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        float DegreeDifference = Mathf.PI * 2 / RippleList.Length;

        for (int i = 0; i < RippleList.Length; i++)
        {
            var test = Instantiate(RippleResource, Vector3.zero, Quaternion.identity);
            test.GetComponent<RippleAgent>().movement = new Vector3(Mathf.Cos(DegreeDifference * i), Mathf.Sin(DegreeDifference * i), 0f);
            RippleList[i] = test;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < RippleList.Length - 1; i++)
        {
            DrawLine(RippleList[i].transform.localPosition, RippleList[i+1].transform.localPosition);
        }

        DrawLine(RippleList[RippleList.Length-1].transform.localPosition, RippleList[0].transform.localPosition);
    }

    private void DrawLine(Vector3 pointA, Vector3 pointB)
    {
        //GameObject lineRendGameObj = Instantiate(lineRendPrefab, transform.position, Quaternion.identity) as GameObject;
        //LineRenderer line = lineRendGameObj.AddComponent("LineRenderer") as LineRenderer;
        //line.material = lineMaterial;
        //line.SetPosition(0, pointA);
        //line.SetWidth(0.45f, 0.45f);
        //float dist = Vector3.Distance(pointA, pointB);
        //Vector3 final = dist * Vector3.Normalize(pointB - pointA) + pointA;
        //line.SetPosition(1, final);

        //Gizmos.DrawLine(pointA, pointB);

        LineRenderer newRender = new GameObject(). as LineRenderer;
        newRender.SetPosition(0, pointA);
        newRender.SetPosition(1, pointB);
    }
}
