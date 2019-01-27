using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Ripple agent is a single node that forms the outline of a ripple. It does
/// a few things:
/// <list type="bullet">
///     <item>It moves in a straight line away from its origin at a fixed
///     speed</item>
///     <item>When it gets too far away from its left neighbor, it spawns a new
///     agent between them, iff they share the same origin.</item>
///     <item>When it collides with a reflector, it reflects itself. It does
///     this by asking its origin to create a mirrored origin (see RippleOrigin).
///     Because agents always move away from their origin, this causes it to
///     move away in the new direction</item>
///     <item>When it gets too far away from its left neighbor, and they don't
///     share an origin, it assumes a disjunction happened at a corner, and
///     runs a bunch of logic to create extra agents with the corner as the
///     origin</item>
/// </summary>
public class RippleAgent : MonoBehaviour
{
    // This Agent's origin. Agents always move at a fixed speed away from this
    // point. It changes when the agent undergoes a reflection.
    public RippleOrigin origin;

    // Set a new origin; update the rigidbody velocity
    private void SetOrigin(RippleOrigin newOrigin)
    {
        this.origin = newOrigin;
        this.GetComponent<Rigidbody2D>().velocity = getRelativeVec().normalized * speed;
    }

    // Our velocity. 
    public float speed;

    public RippleAgent leftNeighbor;
    public RippleAgent rightNeighbor;

    // How far away you can be from a neighbor before spawning a new neighbor.
    public double spawnThreshold = 1;

    // How far away you can be from a neighbor before we assume a corner happened
    // and a new origin is created
    public double cornerThreshold = 3;

    // The vector from the Agent's origin to its location
    private Vector3 getRelativeVec()
    {
        return transform.position - origin.transform.position;
    }

    // Spawn a new Agent to this agent's "Left". Update the chain of leftNeighbor
    // and rightNeighbor
    private RippleAgent SpawnLeft(Vector3 position, RippleOrigin newOrigin)
    {
        var oldLeftNeighbor = leftNeighbor;

        var newLeftNeighbor = Instantiate(this, position, this.transform.rotation, this.transform.parent);
        newLeftNeighbor.SetOrigin(newOrigin);

        oldLeftNeighbor.rightNeighbor = newLeftNeighbor;
        this.leftNeighbor = newLeftNeighbor;

        newLeftNeighbor.leftNeighbor = oldLeftNeighbor;
        newLeftNeighbor.rightNeighbor = this;

        return newLeftNeighbor;
    }

    // Use a slerp calculation to automatically spawn a new agent to this agent's
    // left in the right location along the curve.
    private RippleAgent SpawnLeftOnCurve()
    {
        var myRelativeVec = getRelativeVec();
        var neighborRelativeVec = leftNeighbor.getRelativeVec();
        var newRelativeVec = Vector3.Slerp(myRelativeVec, neighborRelativeVec, 0.5f);

        return SpawnLeft(this.origin.transform.position + newRelativeVec, this.origin);
    }

    private void FixedUpdate()
    {
        // Check if we need to spawn a new neighbor
        var toNeighbor = this.leftNeighbor.transform.position - this.transform.position;
        var distance = toNeighbor.magnitude;
        var sameOrigin = origin == leftNeighbor.origin;
        if (distance > spawnThreshold && sameOrigin) 
        {
            SpawnLeftOnCurve();
        }

        // Check if we hit a corner, and need to create a new corner origin with
        // some new agents
        else if (distance > cornerThreshold && !sameOrigin)
        {
            Vector3 closest1;
            Vector3 closest2;

            // Locate the corner
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

    private void Start()
    {
        var body = this.GetComponent<Rigidbody2D>();
        body.velocity = this.getRelativeVec().normalized * this.speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for collisions with Reflectors, and reflect if necessary.
        var reflector = collision.gameObject.GetComponent<Reflector>();
        var newOrigin = origin.GetReflectedOrigin(reflector);
        SetOrigin(newOrigin);
    }
}
