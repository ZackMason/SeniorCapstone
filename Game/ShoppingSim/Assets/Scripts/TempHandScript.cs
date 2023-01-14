using UnityEngine;
using System.Collections.Generic;

public class TempHandScript : MonoBehaviour
{
    [SerializeField] private string ItemTag;

    private List<Rigidbody> currentItemRigidbodies = new List<Rigidbody>();
    private List<FixedJoint> fixedJoints = new List<FixedJoint>();
    private List<Rigidbody> connectedItemRigidbodies = new List<Rigidbody>();
    private List<int> connectedItemLayers = new List<int>();

    private void Update() {
        if (Input.GetMouseButton(0)) {
            for (int i = 0; i < currentItemRigidbodies.Count; i++) {
                if (!connectedItemRigidbodies.Contains(currentItemRigidbodies[i])) {
                    FixedJoint fixedJoint = transform.parent.gameObject.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = currentItemRigidbodies[i];
                    fixedJoints.Add(fixedJoint);
                    connectedItemRigidbodies.Add(currentItemRigidbodies[i]);
                    connectedItemLayers.Add(currentItemRigidbodies[i].gameObject.layer);
                    currentItemRigidbodies[i].gameObject.layer = 2;
                }

                currentItemRigidbodies.RemoveAt(i);
                i--;
            }

            return;
        }

        for (int i = 0; i < fixedJoints.Count; i++) {
            connectedItemRigidbodies[i].gameObject.layer = connectedItemLayers[i];
            Destroy(fixedJoints[i]);
        }

        fixedJoints.Clear();
        connectedItemRigidbodies.Clear();
        connectedItemLayers.Clear();
    }
    
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == ItemTag) {
            currentItemRigidbodies.Add(collider.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider collider) {
        currentItemRigidbodies.Remove(collider.attachedRigidbody);
    }
}
