using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private float dashSpeedMultiplier;

    [SerializeField] private float dashCoolDown;
    [SerializeField] private float dashDuration;

    //Variables de control de cooldown
    private bool isJumping;
    private bool isDashing;
    private bool canDash;
    private float nextPossibleDashTime;

    //Propiedades
    public float DashCoolDown { get => dashCoolDown; set => dashCoolDown = value; }
    public float DashDuration { get => dashDuration; set => dashDuration = value; }

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        canDash = true;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        JumpInput();
        DashInput();
    }

    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }


    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

    private void DashInput()
    {
        
        if (Input.GetKeyDown(dashKey) && canDash && Time.time >= nextPossibleDashTime)
        {
            nextPossibleDashTime = Time.time + DashCoolDown;
            canDash = false;
            isDashing = true;
            StartCoroutine(dashEvent());
        }
    }

    private IEnumerator dashEvent()
    {
        
        float OGSpeed = movementSpeed;
        movementSpeed *= dashSpeedMultiplier;
        yield return new WaitForSeconds(DashDuration);
        movementSpeed = OGSpeed;
        charController.Move(new Vector3(0,0,0));
        isDashing = false;
        canDash = true;
    }
  

}

    
