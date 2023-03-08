using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    // Set animator controller
    public Animator animator;
    public bool midair;
    public bool rolling;
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public Camera cam;
    public Rigidbody rb;
    public float mouseY = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update player rotation based on mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX, 0);

        // use Input.GetAxis to get the horizontal and vertical axis
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float xVelocity = horizontal;
        float zVelocity = vertical;

        float magnitude = Mathf.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);
        if (magnitude > 1)
        {
            xVelocity /= magnitude;
            zVelocity /= magnitude;
        }

        xVelocity *= speed;
        zVelocity *= speed;

        Vector3 movement = new Vector3(xVelocity, 0, zVelocity);
        movement = transform.rotation * movement;
        movement.y = rb.velocity.y;

        // Move camera to behind and slightly above the player based on the player's rotation and mouse Y movement
        Vector3 camPosition = transform.position;
        camPosition.y += 0.6f;
        camPosition -= transform.forward * 1;
        // Rotate camera around player based on mouse Y movement
        cam.transform.position = camPosition;
        mouseY -= Input.GetAxis("Mouse Y");
        cam.transform.RotateAround(transform.position, transform.right, mouseY);
        cam.transform.LookAt(transform.position + Vector3.up * 0.3f);


        if (!rolling && Input.GetButtonDown("Jump") && midair == false)
        {
            movement.y = jumpSpeed;
            midair = true;
            animator.SetTrigger("Jump");
        }
        else if (midair == true)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 0.1f))
            {
                midair = false;
            }
        }
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 0.1f))
            {
                midair = true;
            }
        }

        if (!midair && Input.GetButtonDown("Fire3") && !rolling)
        {
            rolling = true;
            animator.SetTrigger("Roll");
            Invoke("StopRolling", 0.7f);
        }

        if (rolling)
        {
            movement = transform.forward * speed * 2;
            movement.y = rb.velocity.y;
        }
        rb.velocity = movement;

        animator.SetFloat("xVelocity", xVelocity);
        animator.SetFloat("zVelocity", zVelocity);
        animator.SetFloat("Velocity", magnitude);
        animator.SetBool("Midair", midair);
    }

    void StopRolling()
    {
        rolling = false;
    }
}
