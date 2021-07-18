using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The speed at which the player moves")]
    public float moveSpeed=2f;
    [Tooltip("The speed at which the player rotates to look left and right (calculated in degrees)")]
    public float lookSpeed=60f; 
    [Tooltip("The power with which the player jumps")]
    public float jumpPower=8f;
    [Tooltip("The strength of gravity")]
    public float gravity=9.81f;

    [Header("Jump Timing")]
    public float jumpTimeLeniency=0.1f;
    float timeToStopBeingLenient=0;
    
    [Header("Required Prefrences")]
    [Tooltip("Player shooter script that fires projectiles")]
    public Shooter playerShooter;
    public Health playerHealth;
    public List<GameObject> disabledWhileDead; // to stop us from firing when we're dead 
    bool doubleJumpAvailable=false;
    

    // The charecter controller component on the player
    private CharacterController controller;
    private InputManager inputManager;

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetUpCharacterController();
        SetUpInputManager();
    }

    private void SetUpCharacterController(){
        controller=GetComponent<CharacterController>();
        if(controller == null){
            Debug.LogError("The player controler scrip does not have a character controoler on the same object");
        }
    }
    private void SetUpInputManager(){
        inputManager=InputManager.instance;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        if(playerHealth.currentHealth <=0 ){
            // player dead!
            foreach(GameObject inGameObject in disabledWhileDead){
                inGameObject.SetActive(false);
            }
            return;
        } else{
            foreach(GameObject inGameObject in disabledWhileDead){
                inGameObject.SetActive(true);
            }
        }
        ProcessMovement();
        ProcessRotation();
    }
    Vector3 moveDirection;
    private void ProcessMovement(){
        // Get the input from the inputManager
        float leftRightInput=inputManager.horizontalMoveAxis;
        float forwardBackwardInput=inputManager.verticalMoveAxis;
        bool jumpPressed=inputManager.jumpPressed;


        // Handle the control of the player while it is on the ground
        if(controller.isGrounded){
            doubleJumpAvailable=true;
            timeToStopBeingLenient = Time.time + jumpTimeLeniency;

            // Set the movement direction to be the recived input, set y to 0 since we are on the ground
            moveDirection=new Vector3(leftRightInput, 0, forwardBackwardInput);
            // Set the move direction in relation to the transform, rather than the origin
            moveDirection= transform.TransformDirection(moveDirection);
            moveDirection=moveDirection * moveSpeed;

            if(jumpPressed){
                moveDirection.y=jumpPower;
            }
        }  else{
            // To move while in the air
            moveDirection= new Vector3(leftRightInput * moveSpeed, moveDirection.y, forwardBackwardInput *moveSpeed);
            moveDirection=transform.TransformDirection(moveDirection);

            if(jumpPressed && Time.time < timeToStopBeingLenient){
                moveDirection.y=jumpPower;
            }
            else if(jumpPressed && doubleJumpAvailable){
                moveDirection.y=jumpPower;
                doubleJumpAvailable=false;
            }
        }
        moveDirection.y -=gravity * Time.deltaTime; //insures jump is applied smoothly
        if(controller.isGrounded && moveDirection.y < 0){
            moveDirection.y=-0.3f; // keep gravity from stacking up on player while they're grounded
        }
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ProcessRotation(){
        float horizontalLookInput=inputManager.horizontalLookAxis;
        Vector3 playerRotation= transform.rotation.eulerAngles; // get euler angel, 0 to 300 degrees, becuase it's stored in Quaternion
        transform.rotation=Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime, playerRotation.z ));
    }
}
