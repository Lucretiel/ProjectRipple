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
        float DegreeDifference = Mathf.PI * 2 / RippleList.Length;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.positionCount = RippleList.Length + 1;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")){ color = Color.red };

        for (int i = 0; i < RippleList.Length; i++)
        { 
            RippleList[i] = Instantiate(RippleResource);
            RippleList[i].transform.SetParent(this.transform);
            RippleList[i].transform.localPosition = Vector3.zero;
            RippleList[i].GetComponent<RippleAgent>().movement = new Vector3(Mathf.Cos(DegreeDifference * i), Mathf.Sin(DegreeDifference * i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < RippleList.Length; i++)
        {
            lineRenderer.SetPosition(i, RippleList[i].transform.position);
        }

        lineRenderer.SetPosition(RippleList.Length, RippleList[0].transform.position);
    }

    public void Remove()
    {
        Object.Destroy(gameObject);
    }
}
