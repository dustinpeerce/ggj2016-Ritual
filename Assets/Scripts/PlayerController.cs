using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Public Attributes
    public LayerMask whatIsGround;

    // Private Attributes
    private Camera mainCamera;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private float speed = 8.0f;
    private float jumpForce = 1100.0f;
    private int direction = 1;
    private bool isGrounded = false;

    void Start() {
        mainCamera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
	void Update () {

        // Check for Ground
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 2.5f), 0.1f, whatIsGround) != null) {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
        else {
            isGrounded = false;
            animator.SetBool("Grounded", false);
        }

        // Movement Input
        if (Input.GetMouseButton(0)) {
            if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                direction = 1;
            else
                direction = -1;
        }
        else {
            direction = 0;
        }

        // Jumping Input
        if (Input.GetMouseButtonDown(1)) {
            if (isGrounded) {
                rigidbody.AddForce(new Vector2(0, jumpForce));
            }
        }
	}

    void FixedUpdate() {
        // Move the character
        rigidbody.velocity = new Vector2(direction*speed, rigidbody.velocity.y);

        // Set speed variables for animator
        animator.SetFloat("Speed", rigidbody.velocity.x);
        animator.SetFloat("Vertical Velocity", rigidbody.velocity.y);
    }

}
