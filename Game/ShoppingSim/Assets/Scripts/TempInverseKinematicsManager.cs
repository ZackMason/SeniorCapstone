using UnityEngine;

public class TempInverseKinematicsManager : MonoBehaviour {
    [SerializeField] private string grabbedItemLayer;
    [SerializeField] private Transform jointTransform, jointParentTransform;
    // The code currently assumes that both of the arm's segments have the same length
    [SerializeField] private float segmentLength;
    [SerializeField] private int numOfLayers;
    [SerializeField] private Collider raycastCollider;
    [SerializeField] private float raycastDistance;

    private ConfigurableJoint configurableJoint;
    private Quaternion defaultRotation;

    private void Start() {
        configurableJoint = GetComponent<ConfigurableJoint>();

        defaultRotation = jointTransform.rotation;
    }

    private void FixedUpdate() {
        RaycastHit hitInfo;
        RaycastHit hitInfo2;

        // Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, ((int) (Mathf.Pow(2.0f, numOfLayers)) - 1) ^ LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer), grabbedItemLayer));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Floor"));
        float distance = hitInfo.distance;
        raycastCollider.Raycast(new Ray(ray.GetPoint(raycastDistance), -ray.direction), out hitInfo2, raycastDistance);

        if (hitInfo.collider != null && hitInfo2.distance > distance) {
            // return;
        }

        if (hitInfo2.collider == null) {
            return;
        }

        Vector3 targetPosition = hitInfo2.point;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - jointTransform.position);

        float distanceFromTarget = Vector3.Distance(targetPosition, jointTransform.position);

        if (jointParentTransform != null) {
            distanceFromTarget = Vector3.Distance(targetPosition, jointParentTransform.position);
        }
        
        // This function doesn't seem work for the elbow right now-only for the shoulder. This probably has to do with the elbow starting at an angle
        // I have changed up the function a lot so it might work now
        if (distanceFromTarget < segmentLength * 2.0f) {
            if (jointParentTransform == null) {
                targetRotation = CalculateAdjustedRotation(targetRotation, distanceFromTarget);
            } else {
                targetRotation = Quaternion.LookRotation(targetPosition - jointTransform.position, targetPosition - (jointTransform.position + jointParentTransform.position) * 0.5f);
            }
        }

        configurableJoint.targetRotation = Quaternion.Inverse(jointTransform.rotation) * targetRotation;
    }

    private Quaternion CalculateAdjustedRotation(Quaternion targetRotation, float distanceFromTarget) {
        Quaternion currentRelativeRotation = Quaternion.Inverse(targetRotation) * configurableJoint.targetRotation;

        Vector2 newRelativePosition = currentRelativeRotation * Vector3.forward;
        
        newRelativePosition = newRelativePosition.normalized * Mathf.Sqrt(segmentLength * segmentLength * 4.0f - distanceFromTarget * distanceFromTarget);

        Vector3 newTargetPosition = jointTransform.TransformPoint(new Vector3(newRelativePosition.x, newRelativePosition.y, distanceFromTarget));
        Vector3 newTargetUpPosition = jointTransform.TransformPoint(new Vector3(newRelativePosition.x, newRelativePosition.y, 0.0f));
        
        Quaternion newTargetRotation = targetRotation * Quaternion.LookRotation(newTargetPosition - jointTransform.position, newTargetUpPosition - jointTransform.position);

        return newTargetRotation;
    }
}
