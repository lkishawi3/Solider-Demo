using UnityEngine;

[RequireComponent(typeof(SoldierController))]
public class SoldierMovement : MonoBehaviour 
{
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;
    [SerializeField] private GameObject soldierModel;

    private Camera viewCamera;
    private SoldierController controller;
    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start() 
    {
        controller = GetComponent<SoldierController>();
        viewCamera = Camera.main;
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        // Only handle input in Update
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward * verticalInput;
        Vector3 right = transform.right * horizontalInput;
        moveDirection = (forward + right).normalized;

        HandleRotation();
    }

    void FixedUpdate()
    {
        // Apply movement in FixedUpdate
        Vector3 moveVelocity = moveDirection * moveSpeed;
        controller.Move(moveVelocity);
    }

    void HandleRotation()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance)) 
        {
            Vector3 point = ray.GetPoint(rayDistance);
            
            // Limit rotation range
            Vector3 direction = point - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                if (angle > 90f)
                {
                    targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 90f);
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}