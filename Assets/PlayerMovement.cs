using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float speed = 8f;
    private float _gravity = -19.62f;
    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private Transform groundCheck;
    private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;



    

    public bool isRunning;

    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x == 1 && Input.GetKey(KeyCode.LeftShift) || z == 1 && Input.GetKey(KeyCode.LeftShift)  || x == -1 && Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded)
            {
                speed = 10f;
                
                isRunning = true;
            }
            else
            {
                speed = 6f;
            }

            
        }
        else
        {
            if (isGrounded)
            {
                speed = 8f;
                isRunning = false;
            }
            else
            {
                speed = 5f;
                isRunning = false;
            }
            
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            speed = speed / 1.3f;
        }


        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity);
            
        }

        velocity.y += _gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

   
}
