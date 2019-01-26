using System.Collections.Generic;
using UnityEngine;

public class ShapeTriggerSystem : MonoBehaviour {
    private bool _allShapesTriggered = false;
    private Dictionary<int, bool> _shapeTriggers = new Dictionary<int, bool>();

    private void Awake() {
       var components = GetComponentsInChildren<ShapeTrigger>();

        foreach (var component in components) {
            _shapeTriggers.Add(component.GetInstanceID(), false);
        }
    }

    void Update()
    {
        if (_allShapesTriggered) {
            Debug.Log("Win Condition");
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
}
