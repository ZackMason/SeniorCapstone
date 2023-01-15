using UnityEngine;

public class TempInverseKinematicsManager : MonoBehaviour {
    [SerializeField] private Transform jointTransform;
    // The code currently assumes that both of the arm's segments have the same length
    [SerializeField] private float segmentLength;

    private ConfigurableJoint configurableJoint;
    private Quaternion defaultRotation;

    private void Start() {
        configurableJoint = GetComponent<ConfigurableJoint>();

        defaultRotation = jointTransform.rotation;
    }

    private void FixedUpdate() {
        RaycastHit hitInfo;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

        if (hitInfo.collider == null) {
            return;
        }

        Vector3 targetPosition = hitInfo.point;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - jointTransform.position);

        float distanceFromTarget = Vector3.Distance(targetPosition, jointTransform.position);
        
        // This function doesn't seem work for the elbow right now-only for the shoulder. This probably has to do with the elbow starting at an angle
        // I have changed up the function a lot so it might work now
        if (distanceFromTarget < segmentLength * 2.0f) {
            targetRotation = CalculateAdjustedRotation(targetRotation, distanceFromTarget);
        }

        configurableJoint.targetRotation = Quaternion.Inverse(jointTransform.rotation) * targetRotation;
    }

    private Quaternion CalculateAdjustedRotation(Quaternion targetRotation, float distanceFromTarget) {
        Quaternion currentRelativeRotation = Quaternion.Inverse(targetRotation) * configurableJoint.targetRotation;

        Vector2 newRelativePosition = currentRelativeRotation * Vector3.forward;
        
        newRelativePosition = newRelativePosition.normalized * Mathf.Sqrt(segmentLength * segmentLength * 4.0f - distanceFromTarget * distanceFromTarget);

        Vector3 newTargetPosition = jointTransform.TransformPoint(new Vector3(newRelativePosition.x, newRelativePosition.y, distanceFromTarget));
        
        Quaternion newTargetRotation = targetRotation * Quaternion.LookRotation(newTargetPosition - jointTransform.position);

        return newTargetRotation;
    }
}
