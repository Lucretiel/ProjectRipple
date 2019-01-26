using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleAgent : MonoBehaviour
{
    // Our velocity. 
    public Vector3 velocity;

    public RippleAgent leftNeighbor;
    public RippleAgent rightNeighbor;

    // How far away you can be from a neighbor before spawning a new neighbor.
    // Note that this is a square quantity.
    public double spawnThreshold = 1;

    private void FixedUpdate()
    {
        // TODO: a LOT of this work is going to be duplicated unnecisarily. If
        // that kills performance, move it to a system.
        var myRealPosition = this.transform.position;
        var neighborRealPosition = this.leftNeighbor.transform.position;
        var toNeighbor = neighborRealPosition - myRealPosition;
        var distance = toNeighbor.magnitude;
        if (distance > spawnThreshold)
        {
            //Debug.Break();

            var oldLeftNeighbor = leftNeighbor;
            var midpoint = myRealPosition + (toNeighbor /2);

            // Find the new velocity as the average of the two. It might be better
            // to do a simple rotation?
            var newVelocity = (this.velocity + oldLeftNeighbor.velocity).normalized * this.velocity.magnitude;

            // Perterb the midpoint by h'.
            // Ask Nathan for a proof on this formula
            var newCenter = midpoint;

            var newLeftNeighbor = Instantiate(this);
            newLeftNeighbor.transform.position = newCenter;
            newLeftNeighbor.velocity = newVelocity;

            oldLeftNeighbor.rightNeighbor = newLeftNeighbor;
            this.leftNeighbor = newLeftNeighbor;

            newLeftNeighbor.leftNeighbor = oldLeftNeighbor;
            newLeftNeighbor.rightNeighbor = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += (this.velocity * Time.deltaTime);
    }
}
