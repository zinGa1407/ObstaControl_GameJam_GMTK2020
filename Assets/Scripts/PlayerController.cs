using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject remoteControlModel = null;

    public float maxSpeed = 8f;

    public bool up { get; set; }
    public bool down { get; set; }
    public bool right { get; set; }
    public bool left { get; set; }
    public bool canJump { get; set; }

    //Jump timers
    private float afterFallJumpTimer = 0f;
    private float bunnyHopJumpTimer = 0f;
    private float RESET_JUMP_TIMER = 0.2f;

    private Vector3 jumpForce = new Vector3(0f, 250f, 0f);
    private Vector3 forwardForce = new Vector3(0f, 0f, 10f);
    private Vector3 strafeForce = new Vector3(10f, 0f, 0f);

    private Rigidbody _rb;

    private bool hasRemoteControl = false;


    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Movement Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) up = true;
        else up = false;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) left = true;
        else left = false;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) down = true;
        else down = false;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) right = true;
        else right = false;

        //Remote Control Input
        if(hasRemoteControl)
        {
            if (Input.GetKeyDown(KeyCode.F))
                Debug.Log("Remote Button pressed");
        }

        // Jump Logic
        if (IsGrounded())
        {
            canJump = true;
            afterFallJumpTimer = RESET_JUMP_TIMER;
        }
        else
        {
            if (afterFallJumpTimer >= 0f) afterFallJumpTimer -= Time.deltaTime;
            if (afterFallJumpTimer < 0f)
                canJump = false;
        }

        if (bunnyHopJumpTimer >= 0f) bunnyHopJumpTimer -= Time.deltaTime;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1.05f, LayerMask.NameToLayer("Grouned"));
    }

    private void FixedUpdate()
    {
        //Movement
        Vector3 movementForce = Vector3.zero;
        if (up) movementForce += forwardForce;
        if (down) movementForce -= forwardForce;
        if (right) movementForce += strafeForce;
        if (left) movementForce -= strafeForce;

        //Limit velocity
        if (_rb.velocity.x >= maxSpeed || _rb.velocity.x <= -maxSpeed) movementForce.x = 0f;
        if (_rb.velocity.z >= maxSpeed || _rb.velocity.z <= -maxSpeed) movementForce.z = 0f;

        //Add movement Force to rigidbody
        _rb.AddForce(movementForce, ForceMode.Force);

        //Jump
        if ((Input.GetKey(KeyCode.Space) || bunnyHopJumpTimer > 0f))
        {
            if (canJump)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                _rb.AddForce(jumpForce, ForceMode.Force);
                canJump = false;
            }
            else
            {
                if(bunnyHopJumpTimer < 0f ) bunnyHopJumpTimer = RESET_JUMP_TIMER;
            }
        }
    }

    private void PickupRemoteControl(GameObject pickupObject)
    {
        Destroy(pickupObject);
        hasRemoteControl = true;

        //Activate RemoteControl model in hand
        remoteControlModel.SetActive(true);

        //Activate UI Element for RemoteControl

        //Explain Pressing F
    }

    //Collision checking triggers
    private void OnTriggerEnter(Collider other)
    {
        //Checkpoint trigger
        if (other.tag == "Checkpoint") GameManager.Instance.SetNewCheckpoint(other.GetComponent<Checkpoint>());
        
        //Player dies
        if (other.tag == "KillZone") GameManager.Instance.RespawnPlayer( this.gameObject );

        //Remote control pickup
        if (other.tag == "RemoteControl") PickupRemoteControl(other.gameObject);

    }

}
