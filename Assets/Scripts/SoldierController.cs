using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoldierController : MonoBehaviour 
{
    private Vector3 velocity;
    private Rigidbody rb;
    private float rotationSpeed = 10f;
    private Quaternion targetRotation;
    private Animator animator;
    private float currentSpeed = 0f;
    private float speedSmoothTime = 0.1f;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearDamping = 5f;
        targetRotation = transform.rotation;
        
        // Get animator from child object (the soldier model)
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint) 
    {
        Vector3 direction = lookPoint - transform.position;
        direction.y = 0;
        
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Limit the rotation range
            float angle = Quaternion.Angle(transform.rotation, targetRotation);
            if (angle > 90f)
            {
                targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 90f);
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Update() 
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * 10f * Time.deltaTime
        );
    }

    void FixedUpdate() 
    {
        // Apply velocity to rigidbody only if there is player input
        if (velocity.magnitude > 0.1f)
        {
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
            Debug.Log("Velocity: " + velocity.ToString());

            // Update animator parameters
            if (animator != null)
            {
                // Smoothly interpolate speed parameter based on velocity magnitude
                currentSpeed = Mathf.SmoothDamp(currentSpeed, velocity.magnitude, ref speedSmoothTime, Time.deltaTime);
                animator.SetFloat("Speed", currentSpeed);
                Debug.Log("Speed parameter set to: " + currentSpeed);
            }
        }
        else
        {
            // If no player input, set velocity and speed to zero
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            currentSpeed = 0f;
            
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            
            Debug.Log("No player input. Velocity and speed reset to zero.");
        }
    }
} 