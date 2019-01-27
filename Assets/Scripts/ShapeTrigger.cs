using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using Vector3 = UnityEngine.Vector3;

public class ShapeTrigger : MonoBehaviour {
    public Color TriggerColor = new Color(0, 128, 128); // teal
    public Color OriginalColor;
    
    private SpriteShapeRenderer _spriteRenderer;
    private Dictionary<int, Vector3> _colliderBoundSizes = new Dictionary<int, Vector3>();
    private Vector3 _boundSize;
    private Vector3 _collectiveColliderSize = Vector3.zero;
    private float _autoDieTime = 1;
    [SerializeField]
    private bool _isCheckingCollisions = true;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteShapeRenderer>();
        OriginalColor = _spriteRenderer.material.color;
        _boundSize = _spriteRenderer.bounds.size;
    }

    private void Update() {
        if (_isCheckingCollisions) {
            CheckCollisionIntersection();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        AddCollider(other.GetInstanceID(), other.bounds.size);
    }

    private void OnTriggerExit2D(Collider2D other) {
        RemoveCollider(other.GetInstanceID(), other.bounds.size);
    }

    private void CheckCollisionIntersection() {
        if (_collectiveColliderSize.x >= _boundSize.x && 
            _collectiveColliderSize.y >= _boundSize.y) {
            ChangeColor(TriggerColor);
            SendMessageUpwards("RegisterShapeCollision", GetInstanceID());
        } else if (_spriteRenderer.material.color != OriginalColor) {
            ChangeColor(OriginalColor);
            SendMessageUpwards("UnregisterShapeCollision", GetInstanceID());
        }
    }

    private void ChangeColor(Color color) {
        _spriteRenderer.material.color = color;
    }

    private void StopCheckingCollisions() {
        _isCheckingCollisions = false;
    }
    
    private void AddCollider(int instanceId, Vector3 boundSize) {
        // TODO: this adds the bounds.size on ENTERING, not full take-up of the value. We need to address this.
        _collectiveColliderSize += boundSize;
        _colliderBoundSizes.Add(instanceId, boundSize);
        StartCoroutine(UnregisterDeadCollider(instanceId, boundSize));
    }

    private void RemoveCollider(int instanceId, Vector3 boundSize) {
        _collectiveColliderSize -= boundSize;
        _colliderBoundSizes.Remove(instanceId);
    }

    /*
     * Sort of a hack to cope with a Ripple dying before
     * moving outside of the shape.
     */
    private IEnumerator UnregisterDeadCollider(int instanceId, Vector3 boundSize) {
        yield return new WaitForSeconds(_autoDieTime);
        Vector3 vectorArtifact;
        if (_colliderBoundSizes.TryGetValue(instanceId, out vectorArtifact)) {
            RemoveCollider(instanceId, boundSize);
        }
    }
}
