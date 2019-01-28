using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleManager : MonoBehaviour {
    public float startFadeTime = 2.9f;
    public float autoDieTime = 3;
    public Material shader;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(this.transform);
        lineRenderer.material = shader;
        StartCoroutine(AutoDie());
        StartCoroutine(AutoFade());
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

    private IEnumerator AutoDie()
    {
        yield return new WaitForSeconds(autoDieTime);
        Destroy(gameObject);
    }

    private IEnumerator AutoFade()
    {
        yield return new WaitForSeconds(startFadeTime);
        var startMoment = Time.time;
        var fadeDuration = autoDieTime - startFadeTime;
        var startColor = shader.color;

        while(true)
        {
            // Wait 1 frame
            yield return null;

            // Compute our time delta
            var now = Time.time;
            var duration = now - startMoment;

            // Compute our fade amount
            var fadePercentage = 1 - (duration / fadeDuration);

            // Update the alpha
            var newColor = startColor;
            newColor.a = startColor.a * fadePercentage;
            shader.color = newColor;
        }
    }
}
