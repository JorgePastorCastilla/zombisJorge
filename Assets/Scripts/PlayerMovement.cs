using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    
    public CharacterController controller;
    public float gravity = -9.81f;
    public Vector3 velocity;
    private float speed;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    // GroundCheck
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f; //Umbral de distància enterra
    public LayerMask groundMask;
    
    // Jump
    public float jumpHeight = 2f;
    
    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        
        if (Input.GetButton("Fire3") && isGrounded)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * speed * Time.deltaTime);
        
        

        
        // Mirar si estic tocant el terra
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Jump
        // public float jumpHeight = 2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
