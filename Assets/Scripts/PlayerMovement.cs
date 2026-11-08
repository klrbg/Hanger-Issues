using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.2f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction lookAction;
    private float cameraPitch = 0f;
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        lookAction = new InputAction("Look", InputActionType.Value);
        lookAction.AddBinding("<Mouse>/delta");
        lookAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        direction = direction.normalized;
        direction = transform.TransformDirection(direction);
        
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();
        transform.Rotate(0f, mouseInput.x * mouseSensitivity, 0f);
        cameraPitch += -mouseInput.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 gravityDirection = new Vector3(0f, verticalVelocity, 0f);

        Vector3 walkingVelocity = direction * moveSpeed;

        Vector3 totalVelocity = walkingVelocity + gravityDirection;

        controller.Move(totalVelocity * Time.deltaTime);
    }
}


