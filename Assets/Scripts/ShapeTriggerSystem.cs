using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTriggerSystem : MonoBehaviour {
    public bool Complete = false;
    public float TimeToWaitAfterCompletion = 5.0f;
    [SerializeField]
    private bool _isCheckingCollisions = true;

    private Dictionary<ShapeTriggerWithOrigin, bool> childTriggers = new Dictionary<int, bool>();

    private void Awake() {
       var components = GetComponentsInChildren<ShapeTriggerWithOrigin>();

        foreach (var component in components) {
            childTriggers.Add(component, false);
        }
    }

    public bool allShapesTriggered
    {
        get
        {
            foreach(bool matched in childTriggers.Values)
            {
                if(!matched)
                {
                    return false;
                }
            }
            return true;
        }
    }

    void FixedUpdate()
    {
        if (allShapesTriggered && _isCheckingCollisions) {
            _isCheckingCollisions = false;
            BroadcastMessage("StopCheckingCollisions");
            StartCoroutine(CompleteSystem());
        }
    }

    public void RegisterMatch(ShapeTriggerWithOrigin instanceId) {
        childTriggers[instanceId] = true;
    }

    public void RegisterUnmatch(ShapeTriggerWithOrigin instanceId) {
        childTriggers[instanceId] = false;

    }

    private IEnumerator CompleteSystem() {
        yield return new WaitForSeconds(TimeToWaitAfterCompletion);
        Complete = true;
    }
}
