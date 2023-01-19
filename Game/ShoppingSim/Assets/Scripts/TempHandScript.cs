using UnityEngine;
using System.Collections.Generic;

public class TempHandScript : MonoBehaviour
{
    [SerializeField] private string itemTag, grabbedItemLayer;
    [SerializeField] private Transform leftGripperTransform, rightGripperTransform;
    [SerializeField] private float clampSpeed, triggerSizeAdjustment;
    [SerializeField] private int numOfLayers;

    private List<Rigidbody> touchingItemRigidbodies = new List<Rigidbody>();
    private List<FixedJoint> fixedJoints = new List<FixedJoint>();
    private List<int> connectedItemLayers = new List<int>();

    private BoxCollider boxTrigger;

    private float maxSeperation;

    private void Start() {
        foreach (BoxCollider boxCollider in GetComponents<BoxCollider>()) {
            if (boxCollider.isTrigger) {
                boxTrigger = boxCollider;

                break;
            }
        }

        maxSeperation = Mathf.Abs(rightGripperTransform.localPosition.x - leftGripperTransform.localPosition.x);
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            if (fixedJoints.Count == 0 && touchingItemRigidbodies.Count > 0) {
                foreach (Rigidbody itemRigidbody in touchingItemRigidbodies) {
                    // fixedJoint = transform.parent.gameObject.AddComponent<FixedJoint>();
                    FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>() as FixedJoint;

                    fixedJoint.connectedBody = itemRigidbody;

                    fixedJoints.Add(fixedJoint);
                    connectedItemLayers.Add(fixedJoint.connectedBody.gameObject.layer);
                    fixedJoint.connectedBody.gameObject.layer = LayerMask.NameToLayer(grabbedItemLayer);
                }
            }

            return;
        }

        
        for (int i = 0; i < fixedJoints.Count; i++) {
            fixedJoints[i].connectedBody.gameObject.layer = connectedItemLayers[i];
            
            Destroy(fixedJoints[i]);
        }

        fixedJoints.Clear();
        connectedItemLayers.Clear();
    }

    private void FixedUpdate() {
        RaycastHit hitInfo;
        float distance;
        bool intersected;

        Vector3 newLeftGripperLocalPos;
        Vector3 newRightGripperLocalPos;

        if (Input.GetMouseButton(0)) {
            intersected = Physics.BoxCast(leftGripperTransform.position, leftGripperTransform.lossyScale * 0.5f, leftGripperTransform.right, out hitInfo, leftGripperTransform.rotation, clampSpeed * Time.fixedDeltaTime);

            if (intersected) {
                distance = hitInfo.distance;
            } else {
                distance = clampSpeed * Time.fixedDeltaTime;
                
                if (Physics.CheckBox(leftGripperTransform.position, leftGripperTransform.lossyScale * 0.5f, leftGripperTransform.rotation, ((int) (Mathf.Pow(2.0f, numOfLayers)) - 1) ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)))) {
                    print(((int) (Mathf.Pow(2.0f, numOfLayers)) - 1) ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)));
                    distance = 0.0f;
                }
            }

            newLeftGripperLocalPos = leftGripperTransform.localPosition + Vector3.right * distance;

            intersected = Physics.BoxCast(rightGripperTransform.position, rightGripperTransform.lossyScale * 0.5f, -rightGripperTransform.right, out hitInfo, rightGripperTransform.rotation, clampSpeed * Time.fixedDeltaTime);
            
            if (intersected) {
                distance = hitInfo.distance;
            } else {
                distance = clampSpeed * Time.fixedDeltaTime;

                if (Physics.CheckBox(rightGripperTransform.position, rightGripperTransform.lossyScale * 0.5f, rightGripperTransform.rotation, ((int) (Mathf.Pow(2.0f, numOfLayers)) - 1) ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)))) {
                    distance = 0.0f;
                }
            }

            newRightGripperLocalPos = rightGripperTransform.localPosition + Vector3.left * distance;
        } else {
            intersected = Physics.BoxCast(leftGripperTransform.position, leftGripperTransform.lossyScale * 0.5f, leftGripperTransform.position - rightGripperTransform.position, out hitInfo, leftGripperTransform.rotation, clampSpeed * Time.fixedDeltaTime);

            if (intersected) {
                distance = hitInfo.distance;
            } else {
                distance = clampSpeed * Time.fixedDeltaTime;
            }
            newLeftGripperLocalPos = leftGripperTransform.localPosition + (leftGripperTransform.localPosition - rightGripperTransform.localPosition).normalized * distance;

            intersected = Physics.BoxCast(rightGripperTransform.position, rightGripperTransform.lossyScale * 0.5f, rightGripperTransform.position - leftGripperTransform.position, out hitInfo, rightGripperTransform.rotation, clampSpeed * Time.fixedDeltaTime);
            if (intersected) {
                distance = hitInfo.distance;
            } else {
                distance = clampSpeed * Time.fixedDeltaTime;
            }
            newRightGripperLocalPos = rightGripperTransform.localPosition + (rightGripperTransform.localPosition - leftGripperTransform.localPosition).normalized * distance;
        }

        newLeftGripperLocalPos.x = Mathf.Clamp(newLeftGripperLocalPos.x, -maxSeperation * 0.5f, -leftGripperTransform.lossyScale.x * 0.5f);
        newRightGripperLocalPos.x = Mathf.Clamp(newRightGripperLocalPos.x, rightGripperTransform.lossyScale.x * 0.5f, maxSeperation * 0.5f);

        leftGripperTransform.localPosition = newLeftGripperLocalPos;
        rightGripperTransform.localPosition = newRightGripperLocalPos;

        Vector3 boxTriggerPos = (leftGripperTransform.localPosition + rightGripperTransform.localPosition) * 0.5f;
        boxTriggerPos.y += triggerSizeAdjustment;
        Vector3 boxTriggerSize = leftGripperTransform.localPosition - rightGripperTransform.localPosition;
        boxTriggerSize.x = Mathf.Max(0.0f, Mathf.Abs(boxTriggerSize.x) - Mathf.Max(leftGripperTransform.localScale.x, rightGripperTransform.localScale.x));
        boxTriggerSize.y = Mathf.Max(0.0f, Mathf.Abs(boxTriggerSize.y) + Mathf.Max(leftGripperTransform.localScale.y, rightGripperTransform.localScale.y) + triggerSizeAdjustment * 4.0f);
        boxTriggerSize.z = Mathf.Max(0.0f, Mathf.Abs(boxTriggerSize.z) + Mathf.Max(leftGripperTransform.localScale.z, rightGripperTransform.localScale.z) + triggerSizeAdjustment * 2.0f);

        boxTrigger.center = boxTriggerPos;
        boxTrigger.size = boxTriggerSize;
    }
    
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == itemTag) {
            touchingItemRigidbodies.Add(collider.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider collider) {
        touchingItemRigidbodies.Remove(collider.attachedRigidbody);
    }
}

