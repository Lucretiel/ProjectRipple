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
    public RippleOrigin origin;

    private Vector3 getRelativeVec()
    {
        return transform.position - origin.transform.position;
    }

    private void FixedUpdate()
    {
        // Check for spawning neighbors
        var toNeighbor = this.leftNeighbor.transform.position - this.transform.position;
        var distance = toNeighbor.magnitude;
        if (distance > spawnThreshold && origin.Equals(leftNeighbor.origin))
        {
            var oldLeftNeighbor = leftNeighbor;

            // TODO: this should only happen if they share an origin. Try to use
            // transforms for Ident
            var myRelativeVec = getRelativeVec();
            var neighborRelativeVec = oldLeftNeighbor.getRelativeVec();

            var newRelativeVec = Vector3.Slerp(myRelativeVec, neighborRelativeVec, 0.5f);
 
            var newLeftNeighbor = Instantiate(this, this.origin.transform.position + newRelativeVec, this.transform.rotation, this.transform.parent);

            oldLeftNeighbor.rightNeighbor = newLeftNeighbor;
            this.leftNeighbor = newLeftNeighbor;

            newLeftNeighbor.leftNeighbor = oldLeftNeighbor;
            newLeftNeighbor.rightNeighbor = this;
        }

        // Check for reflections

    }

    // Update is called once per frame
    void Update()
    {
        var tick = (this.transform.position - this.origin.transform.position).normalized * speed * Time.deltaTime;
        this.transform.position += tick; 
        Debug.DrawLine(transform.position, leftNeighbor.transform.position, Color.cyan);
    }

    private void OnTriggerEnter(Collider other)
    {
        var reflector = other.GetComponent<Reflector>();
        var newOrigin = origin.GetReflectedOrigin(reflector);
        origin = newOrigin;
    }
}
