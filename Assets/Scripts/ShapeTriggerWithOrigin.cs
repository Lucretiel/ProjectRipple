using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.U2D;
using UnityEngine;

public class ShapeTriggerWithOrigin : MonoBehaviour
{
    public SphereCollider acceptableOriginZone;
    private HashSet<RippleAgent> currentAgents;

    public Color triggerColor = Color.cyan;
    public Color originalColor;

    private SpriteShapeRenderer spriteRenderer;
    private bool isMatch = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteShapeRenderer>();
        originalColor = spriteRenderer.material.color;
        currentAgents = new HashSet<RippleAgent>();
    }

    private void FixedUpdate()
    {
        CheckSuccess();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var agent = collision.GetComponent<RippleAgent>();
        if (agent == null) return;

        // Check that the agent's origin is inside our acceptable zone
        if(acceptableOriginZone.bounds.Contains(agent.origin.transform.position)) {
            currentAgents.Add(agent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var agent = collision.GetComponent<RippleAgent>();
        if (agent == null) return;

        currentAgents.Remove(agent);
    }

    public bool IsMatch()
    {
        return isMatch;
    }

    // Check if the success state changed; update the color if so.
    private void CheckSuccess()
    {
        // Clear out dead references first. The conversion of these refs to
        // null apparently just happens magically by Unity.
        currentAgents.RemoveWhere(obj => obj == null);

        bool oldMatch = isMatch;
        isMatch = currentAgents.Count > 0;

        if(oldMatch != isMatch)
        {
            if(isMatch)
            {
                spriteRenderer.material.color = triggerColor;
                SendMessageUpwards("RegisterMatch", this);
            }
            else
            {
                spriteRenderer.material.color = originalColor;
                SendMessageUpwards("RegisterUnmatch", this);
            }
        }
    }
}
