using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject remoteControlModel = null;

    [SerializeField]
    private GameObject remoteControlParticle = null;

    private float moveSpeedMultiplier = 10f;
    private float maxSpeed = 4f;

    public bool canJump;

    //Jump timers
    private float afterFallJumpTimer = 0f;
    private float bunnyHopJumpTimer = 0f;
    private float RESET_JUMP_TIMER = 0.2f;
    private float justJumpedTimer = 0.2f;
    private float JUMP_COOLDOWN_TIMER = 0.2f;


    private Vector3 jumpForce = new Vector3(0f, 250f, 0f);
    private float horizontal;
    private float vertical;

    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator = null;
    private string lastAnimState = "Idle";

    [SerializeField]
    private bool hasRemoteControl = false;

    private List<IObstacle> interactingObstacles = new List<IObstacle>();

    Vector3[] rays = new[]
    {
        Vector3.zero,
        new Vector3(0f, 0f, 0.25f),
        new Vector3(0.25f, 0f, 0.25f),
        new Vector3(0.25f, 0f, 0f),
        new Vector3(0.25f, 0f, -0.25f),
        new Vector3(0f, 0f, -0.25f),
        new Vector3(-0.25f, 0f, -0.25f),
        new Vector3(-0.25f, 0f, 0f),
        new Vector3(-0.25f, 0f, 0.25f)
    };

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.gameState == GameManager.GameState.PLAY)
            {
                //Movement Input
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");

                //Remote Control Input
                if (hasRemoteControl)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (interactingObstacles.Count > 0)
                        {
                            foreach (var _obstacle in interactingObstacles)
                            {
                                _obstacle.Interaction(true);
                            }
                        }
                    }
                }

                // Jump Logic
                if (justJumpedTimer <= 0f)
                {
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
                }

                //JUMP
                if ((Input.GetKeyDown(KeyCode.Space) || bunnyHopJumpTimer > 0f))
                {
                    if (canJump)
                    {
                        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                        _rb.AddForce(jumpForce, ForceMode.Force);
                        canJump = false;
                        SoundManager.Instance.PlaySoundEffect("jump");

                        justJumpedTimer = JUMP_COOLDOWN_TIMER;

                        if (lastAnimState != "Jumping")
                        {
                            _animator.SetBool(lastAnimState, false);
                            _animator.SetBool("Jumping", true);
                            lastAnimState = "Jumping";
                        }
                        else _animator.Play("Jumping", 0, 0f);
                    }
                    else
                    {
                        if (bunnyHopJumpTimer < 0f) bunnyHopJumpTimer = RESET_JUMP_TIMER;
                    }
                }

                if (bunnyHopJumpTimer >= 0f) bunnyHopJumpTimer -= Time.deltaTime;
                if (justJumpedTimer >= 0f) justJumpedTimer -= Time.deltaTime;

                //Respawn Player
                if (Input.GetKeyDown(KeyCode.R)) GameManager.Instance.RespawnPlayer(this.gameObject, _rb);
            }

            //Ingame Menu
            if (Input.GetKeyDown(KeyCode.Escape)) GameManager.Instance.ShowIngameMenu();
        }
    }



    private void FixedUpdate()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.gameState == GameManager.GameState.PLAY)
            {
                //Movement
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                //Limit velocity
                //if (_rb.velocity.x >= maxSpeed || _rb.velocity.x <= -maxSpeed) movementForce.x = 0f;
                //if (_rb.velocity.z >= maxSpeed || _rb.velocity.z <= -maxSpeed) movementForce.z = 0f;

                //Add movement Force to rigidbody
                if (IsGrounded())
                {
                    if (Mathf.Abs(_rb.velocity.z) < maxSpeed && Mathf.Abs(_rb.velocity.x) < maxSpeed)
                        _rb.AddForce(direction * moveSpeedMultiplier * 2f, ForceMode.Force);
                    if (direction == Vector3.zero) _rb.velocity = new Vector3(_rb.velocity.x * 0.8f, _rb.velocity.y, _rb.velocity.z * 0.8f);
                }
                else // Air control
                {
                    if (Mathf.Abs(_rb.velocity.z) <= maxSpeed || Mathf.Abs(_rb.velocity.x) <= maxSpeed)
                    {
                        if (Vector3.Dot(_rb.velocity.normalized, direction) > 0.5f)
                        {
                            _rb.AddForce(direction * moveSpeedMultiplier, ForceMode.Acceleration);
                        }
                        else _rb.AddForce(direction * moveSpeedMultiplier * 2f, ForceMode.Acceleration);

                        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), _rb.velocity.y, Mathf.Clamp(_rb.velocity.z, -maxSpeed, maxSpeed));
                    }
                    //Slow down velocity to make movement work again    
                    else _rb.velocity = new Vector3(_rb.velocity.x * 0.9f, _rb.velocity.y, _rb.velocity.z * 0.9f);
                }

                ////Jump
                //if ((Input.GetKeyDown(KeyCode.Space) || bunnyHopJumpTimer > 0f))
                //{
                //    if (canJump)
                //    {
                //        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                //        _rb.AddForce(jumpForce, ForceMode.Force);
                //        canJump = false;
                //        SoundManager.Instance.PlaySoundEffect("jump");

                //        if (lastAnimState != "Jumping")
                //        {
                //            _animator.SetBool(lastAnimState, false);
                //            _animator.SetBool("Jumping", true);
                //            lastAnimState = "Jumping";
                //        }
                //        else _animator.Play("Jumping", 0, 0f);
                //    }
                //    else
                //    {
                //        if (bunnyHopJumpTimer < 0f) bunnyHopJumpTimer = RESET_JUMP_TIMER;
                //    }
                //}

                PlayerAnimationControl();
            }
        }
    }

    private void PickupRemoteControl(GameObject pickupObject)
    {
        Destroy(pickupObject.transform.parent.gameObject);
        hasRemoteControl = true;

        //Activate RemoteControl model in hand
        remoteControlModel.SetActive(true);

        //Activate UI Element for RemoteControl
        UIManager.Instance.AddFeedbackText("You have picked up a remote control!");
        UIManager.Instance.ActivateUIElement("RemoteControl");

        SoundManager.Instance.PlaySoundEffect("remote");
    }

    //Collision checking triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            IObstacle obstacle = other.transform.parent.gameObject.GetComponent<IObstacle>();
            interactingObstacles.Add(obstacle);
            obstacle.ShowFeedbackText(true);
            if (GameManager.Instance.GetRemoteBroken()) obstacle.Interaction(true);
        }

        //Checkpoint trigger
        if (other.tag == "Checkpoint") GameManager.Instance.SetNewCheckpoint(other.GetComponent<Checkpoint>());
        
        //Player dies
        if (other.tag == "KillZone") GameManager.Instance.RespawnPlayer( this.gameObject, _rb );

        //Remote control pickup
        if (other.tag == "RemoteControl") PickupRemoteControl(other.gameObject);


        if (other.tag == "MovingPlatform") gameObject.transform.parent = other.gameObject.transform;


        if (other.tag == "BrokenController")
        {
            GameManager.Instance.SetControllerBroken();
            Destroy(other.gameObject);
            remoteControlParticle.SetActive(true);
            SoundManager.Instance.PlaySoundEffect("broken");
        }

        if (other.tag == "Finish")
        {
            GameManager.Instance.FinishReached();
            SoundManager.Instance.PlaySoundEffect("finish");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            IObstacle obstacle = other.transform.parent.gameObject.GetComponent<IObstacle>();
            obstacle.Interaction(false);
            obstacle.ShowFeedbackText(false);
            interactingObstacles.Remove(obstacle);
        }

        if (other.tag == "MovingPlatform") gameObject.transform.parent = null;
    }

    private void PlayerAnimationControl()
    {
        if (lastAnimState == "Jumping" && !canJump) return;
        if ((Mathf.Abs(_rb.velocity.z) > Mathf.Abs(_rb.velocity.x)) && Mathf.Abs(_rb.velocity.z) > 0.3f)      //Forward or Backward anim
        {
            if (_rb.velocity.z > 0.3f)
            {
                if (lastAnimState != "RunForward")
                {
                    _animator.SetBool(lastAnimState, false);
                    _animator.SetBool("RunForward", true);
                    lastAnimState = "RunForward";
                }
            }
            else if (_rb.velocity.z < -0.3f)
            {
                if (lastAnimState != "RunBackward")
                {
                    _animator.SetBool(lastAnimState, false);
                    _animator.SetBool("RunBackward", true);
                    lastAnimState = "RunBackward";
                }
            }
        }
        else if (_rb.velocity.x > 0.3f)
        {
            if (lastAnimState != "StrafeRight")
            {
                _animator.SetBool(lastAnimState, false);
                _animator.SetBool("StrafeRight", true);
                lastAnimState = "StrafeRight";
            }
        }
        else if (_rb.velocity.x < -0.3f)
        {
            if (lastAnimState != "StrafeLeft")
            {
                _animator.SetBool(lastAnimState, false);
                _animator.SetBool("StrafeLeft", true);
                lastAnimState = "StrafeLeft";
            }
        }
        else //Idle
        {
            if (horizontal == 0f && vertical == 0f)
            {
                if (lastAnimState != "Idle")
                {
                    _animator.SetBool(lastAnimState, false);
                    _animator.SetBool("Idle", true);
                    lastAnimState = "Idle";
                }
            }
        }
            
    }

    private bool IsGrounded()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Wall");

        layerMask = ~layerMask;

        foreach (var ray in rays)
        {
            Debug.DrawRay(transform.position - ray, Vector3.down * 1.02f, Color.red);
            if (Physics.Raycast(transform.position - ray, Vector3.down, out RaycastHit hit, 1.1f, layerMask))
            {
                 return true;
            }
            
        }
        return false;
    }

}
