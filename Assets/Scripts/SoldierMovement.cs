using UnityEngine;

[RequireComponent(typeof(SoldierController))]
public class SoldierMovement : MonoBehaviour 
{
    public float moveSpeed = 5f;
    [SerializeField] private GameObject soldierModel;

    private Camera viewCamera;
    private SoldierController controller;
    private Rigidbody rb;

    void Start() 
    {
        controller = GetComponent<SoldierController>();
        viewCamera = Camera.main;
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        
        // Removed the forced position setting to respect editor position
    }

    void Update() 
    {
        // Handle movement input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        // Handle rotation towards mouse position
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance)) 
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }
    }
} 