
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform holdPoint;

    [SerializeField] private float followSpeed = 12f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private ClothingRack clothingRack;
    [SerializeField] private float maxHoldDistance = 1.5f;

    private Rigidbody heldBody;
    private Collider heldCollider;
    private CharacterController playerCollider;
    private CollisionDetectionMode previousCollisionMode;


    void Start()
    {
        playerCollider = GetComponent<CharacterController>();
    }

    public bool IsHoldingItem()
    {
        return heldBody != null;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame
            && heldBody == null)
        {
            if (Physics.SphereCast(
                cameraTransform.position,
                0.18f,
                cameraTransform.forward,
                out RaycastHit hit,
                3f,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("PickupItem")
                    && hit.rigidbody != null)
                {   
                    ClothingItem itemData = hit.rigidbody.GetComponent<ClothingItem>();
                    
                    if (itemData != null
                        && !itemData.IsCounted())
                    {
                        PickUp(hit.rigidbody);
                    }
                    
                }
            }
        }

        if (Keyboard.current.qKey.wasPressedThisFrame
            && heldBody != null)
        {   
            if (clothingRack != null
                && clothingRack.TryHang(heldBody))
            {
                if (heldCollider != null
                    && playerCollider != null)
                {
                    Physics.IgnoreCollision(heldCollider, playerCollider, false);
                }

                heldBody = null;
                heldCollider = null;
            }
            else
            {
                Drop();
            }
                
            
        }
    }

    void PickUp(Rigidbody body)
    {
        heldBody = body;
        heldCollider = body.GetComponent<Collider>();

        previousCollisionMode = body.collisionDetectionMode;

        body.isKinematic = false;
        body.useGravity = false;
        body.collisionDetectionMode =
            CollisionDetectionMode.ContinuousSpeculative;

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        // Ignoruj len koliziu s vlastnym hracom.
        if (heldCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(
                heldCollider, playerCollider, true);
        }
    }

    void FixedUpdate()
    {
        if (heldBody == null) return;

        Vector3 difference =
            holdPoint.position - heldBody.position;

        if (difference.magnitude > maxHoldDistance)
        {
            Drop();
            return;
        }

        heldBody.linearVelocity = Vector3.ClampMagnitude(
            difference * followSpeed,
            maxSpeed);

        // Otacanie tricka smerom ku kamere.
        Quaternion differenceRotation =
            holdPoint.rotation *
            Quaternion.Inverse(heldBody.rotation);

        differenceRotation.ToAngleAxis(
            out float angle,
            out Vector3 axis);

        if (angle > 180f)
            angle -= 360f;

        heldBody.angularVelocity = Vector3.ClampMagnitude(
            axis * (angle * Mathf.Deg2Rad * rotationSpeed),
            6f);
    }

    void Drop()
    {
        if (heldCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(
                heldCollider, playerCollider, false);
        }

        heldBody.linearVelocity = Vector3.zero;
        heldBody.angularVelocity = Vector3.zero;
        heldBody.useGravity = true;
        heldBody.collisionDetectionMode =
            previousCollisionMode;

        heldBody = null;
        heldCollider = null;
    }
}
