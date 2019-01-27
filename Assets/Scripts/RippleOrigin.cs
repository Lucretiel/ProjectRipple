using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleOrigin : MonoBehaviour
{
    private Dictionary<Reflector, RippleOrigin> storedReflections;

    // Get a new origin, reflected about the reflector. We assume the reflector
    // is a Plane with its normal in the +Z direction. We store the reflected
    // origin, so when subsequent points reflect about the same reflector, they
    // all get the same origin.
    public RippleOrigin GetReflectedOrigin(Reflector reflector)
    {
        RippleOrigin resultOrigin;

        if (!storedReflections.TryGetValue(reflector, out resultOrigin))
        {
            var plane = reflector.GetPlane();
            var planeIntersect = plane.ClosestPointOnPlane(this.transform.position);
            var toPlane = planeIntersect - transform.position;
            var oppositePoint = transform.position + toPlane + toPlane;

            resultOrigin = Instantiate(this, oppositePoint, this.transform.rotation, this.transform);
            storedReflections.Add(reflector, resultOrigin);
        }

        return resultOrigin;
    }
    // Start is called before the first frame update
    void Start()
    {
        storedReflections = new Dictionary<Reflector, RippleOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
