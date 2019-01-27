using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using Vector3 = UnityEngine.Vector3;

public class ShapeTrigger : MonoBehaviour {
    private SpriteShapeRenderer _spriteRenderer;
    private Dictionary<int, Collider2D> _colliders = new Dictionary<int, Collider2D>();
    private Vector3 _boundSize;
    private Vector3 _collectiveColliderSize = Vector3.zero;
    private Color _originalColor;
    private float _autoDieTime = 1;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteShapeRenderer>();
        _originalColor = _spriteRenderer.material.color;
        _boundSize = _spriteRenderer.bounds.size;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        AddCollider(other);
        ChangeColor();
    }

    private void OnTriggerExit2D(Collider2D other) {
        RemoveCollider(other);
        ChangeColor();
    }

    private void ChangeColor() {
        if (_collectiveColliderSize.x >= _boundSize.x && 
            _collectiveColliderSize.y >= _boundSize.y) {
            _spriteRenderer.material.color = Color.red;
            SendMessageUpwards("RegisterShapeCollision", GetInstanceID());
        } else if (_spriteRenderer.material.color != _originalColor) {
            _spriteRenderer.material.color = _originalColor;
            SendMessageUpwards("UnregisterShapeCollision", GetInstanceID());
        }
    }
    
    private void AddCollider(Collider2D other) {
        // TODO: this adds the bounds.size on ENTERING, not full take-up of the value. We need to address this.
        _collectiveColliderSize += other.bounds.size;
        _colliders[other.GetInstanceID()] = other;
        StartCoroutine(AutoDie(other));
    }

    private void RemoveCollider(Collider2D other) {
        _collectiveColliderSize -= other.bounds.size;
        _colliders.Remove(other.GetInstanceID());
    }

    private IEnumerator AutoDie(Collider2D other) {
        yield return new WaitForSeconds(_autoDieTime);
        if (_colliders[other.GetInstanceID()] != null) {
            RemoveCollider(other);
        }
    }
}
