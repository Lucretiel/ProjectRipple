using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTriggerSystem : MonoBehaviour {
    public bool Complete = false;
    public float TimeToWaitAfterCompletion = 5.0f;
    [SerializeField]
    private bool _allShapesTriggered = false;
    [SerializeField]
    private bool _isCheckingCollisions = true;

    private Dictionary<int, bool> _shapeTriggers = new Dictionary<int, bool>();

    private void Awake() {
       var components = GetComponentsInChildren<ShapeTrigger>();

        foreach (var component in components) {
            _shapeTriggers.Add(component.GetInstanceID(), false);
        }
    }

    void Update()
    {
        if (_allShapesTriggered && _isCheckingCollisions) {
            _isCheckingCollisions = false;
            BroadcastMessage("StopCheckingCollisions");
            StartCoroutine(CompleteSystem());
        }
    }

    public void RegisterShapeCollision(int instanceId) {
        _shapeTriggers[instanceId] = true;
        _allShapesTriggered = true;
        foreach (var shape in _shapeTriggers) {
            if (!shape.Value) {
                _allShapesTriggered = false;
            }
        }
    }

    public void UnregisterShapeCollision(int instanceId) {
        _shapeTriggers[instanceId] = false;
        _allShapesTriggered = false;
    }

    private IEnumerator CompleteSystem() {
        yield return new WaitForSeconds(TimeToWaitAfterCompletion);
        Complete = true;
    }
}
