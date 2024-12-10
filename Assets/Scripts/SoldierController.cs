using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoldierController : MonoBehaviour 
{
    private Vector3 velocity;
    private Rigidbody rb;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint) 
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
} 