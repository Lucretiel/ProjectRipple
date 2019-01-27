using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleAgent : MonoBehaviour
{
    // Our velocity. 
    public float speed;

    public RippleAgent leftNeighbor;
    public RippleAgent rightNeighbor;

    // How far away you can be from a neighbor before spawning a new neighbor.
    // Note that this is a square quantity.
    public double spawnThreshold = 1;

    // How far away you can be from a neighbor before we assume a corner happened
    // and a new origin is created
    public double cornerThreshold = 3;
    public RippleOrigin origin;

    private Vector3 getRelativeVec()
    {
        return transform.position - origin.transform.position;
    }

    private RippleAgent SpawnLeft(Vector3 position, RippleOrigin newOrigin)
    {
        var oldLeftNeighbor = leftNeighbor;

        var newLeftNeighbor = Instantiate(this, position, this.transform.rotation, this.transform.parent);
        newLeftNeighbor.origin = newOrigin;

        oldLeftNeighbor.rightNeighbor = newLeftNeighbor;
        this.leftNeighbor = newLeftNeighbor;

        newLeftNeighbor.leftNeighbor = oldLeftNeighbor;
        newLeftNeighbor.rightNeighbor = this;

        return newLeftNeighbor;
    }

    private RippleAgent SpawnLeftOnCurve()
    {
        var myRelativeVec = getRelativeVec();
        var neighborRelativeVec = leftNeighbor.getRelativeVec();
        var newRelativeVec = Vector3.Slerp(myRelativeVec, neighborRelativeVec, 0.5f);

        return SpawnLeft(this.origin.transform.position + newRelativeVec, this.origin);
    }

    private void FixedUpdate()
    {
        var toNeighbor = this.leftNeighbor.transform.position - this.transform.position;
        var distance = toNeighbor.magnitude;
        var sameOrigin = origin.Equals(leftNeighbor.origin);
        if (distance > spawnThreshold && sameOrigin)
        {
            SpawnLeftOnCurve();
        }

        // Check for corners
        else if (distance > cornerThreshold && !sameOrigin)
        {
            // Locate the corner
            Vector3 closest1;
            Vector3 closest2;

            Util.ClosestPointsOnTwoLines(
                out closest1, out closest2,
                origin.transform.position, getRelativeVec(),
                leftNeighbor.origin.transform.position, leftNeighbor.getRelativeVec());
            Vector3 corner = Vector3.Lerp(closest1, closest2, 0.5f);

            // Create a new origin at the corner
            var cornerOrigin = Instantiate(this.origin, corner, this.origin.transform.rotation, this.origin.transform.parent);

            var neighborPosition = this.leftNeighbor.transform.position;

            // Clone self and the neighbor, using the new origin
            var selflikeClone = this.SpawnLeft(this.transform.position, cornerOrigin);
            selflikeClone.SpawnLeft(neighborPosition, cornerOrigin);
            selflikeClone.SpawnLeftOnCurve();
        }

    }

    // Update is called once per frame
    void Update()
    {
        var tick = getRelativeVec().normalized * speed * Time.deltaTime;
        this.transform.position += tick;

        if(this.origin.Equals(leftNeighbor.origin))
        {
            Debug.DrawLine(transform.position, leftNeighbor.transform.position, Color.cyan);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var reflector = other.GetComponent<Reflector>();
        var newOrigin = origin.GetReflectedOrigin(reflector);
        origin = newOrigin;
    }
}
