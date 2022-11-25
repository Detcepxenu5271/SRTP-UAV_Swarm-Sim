using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Mesh mesh;
    Rigidbody _rigidbody;

    private void OnDrawGizmos() {
        // Gizmos.color = Color.green;
        // Gizmos.DrawWireMesh(mesh);
    }

    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update() {
        _rigidbody.AddForce(new Vector3(1, 1, 1), ForceMode.Force);
    }
}
