using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class AnimationAndMovmentController : MonoBehaviour
{

    PlayerInput playerInput;
    public CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;


    public Vector2 currentMovementInput;
   public Vector3 currentMovement;
    public Vector3 currentRunMovement;
    bool isMovementPressed;
    public bool isRunPressed;
    float roationFactorPerFrame = 15.0f;
    float runMultiplier = 5.0f;


    void Awake()
    {

        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");



        playerInput.CharacterControls.Move.started += onMomevementInput;
        playerInput.CharacterControls.Move.canceled += onMomevementInput;
        playerInput.CharacterControls.Move.performed += onMomevementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
    }



    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();


    }
    void onMomevementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }

    void handleGravity()
    {

        if (characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;

        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;

        }
    }
    void handleRotation()
    {
        Vector3 positionToLookAt;
        //w która strone postac powinna spoglądać 
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        // obecna rotacja postaci 
        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            //tworzenie nowej rotacji za pośrednictwem przycisków wciskanych przez gracza
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, roationFactorPerFrame * Time.deltaTime);
        }
    }
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
    void Update()
    {
       

        handleGravity();
        handleRotation();
        handleAnimation();

        if(isRunPressed){
             characterController.Move(currentRunMovement * Time.deltaTime *5);
        }else{
             characterController.Move(currentMovement * Time.deltaTime * 5);
        }
        
        
    }
    public void move( Vector2 currentMovement ){
       
       characterController.Move(currentMovement * Time.deltaTime * 5);
       
        //  if(isRunPressed){
        //      characterController.Move(currentRunMovement * Time.deltaTime *5);
        // }else{
        //      characterController.Move(currentMovement * Time.deltaTime * 5);
        // }

    }
    
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();

    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
