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

    void Start() {
        mainCamera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
	void Update () {

        if (Input.GetMouseButton(0)) {
            animator.SetBool("Moving", true);

            if (mainCamera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
                direction = 1;
            else
                direction = -1;
            
            transform.Translate(direction * speed * Time.deltaTime, 0, 0);
        }
        else {
            animator.SetBool("Moving", false);
        }

        if (Input.GetMouseButtonDown(1)) {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 2.5f), 0.1f, whatIsGround) != null) {
                rigidbody.AddForce(new Vector2(0, jumpForce));
            }
        }
	}

    void Flip() {
        Debug.Log("Flipping");
        transform.localScale = new Vector3(direction*transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

}
