using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 5F;
    [SerializeField] private float baseSpeed = 5F;
    [SerializeField] private float jumpHeight = 2F;
    [SerializeField] private float gravity = -9.8F;
    [SerializeField] private bool shouldFaceMoveDirection = true;

    [SerializeField] private Animator playerAnimator;

    private CharacterController controller;
    private Vector3 moveInput;
    private Vector3 velocity;

    private AudioSource jogAudioSource;
    private AudioSource jumpAudioSource;
    private AudioSource placeAudioSource;
    private AudioSource buffAudioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        baseSpeed = speed;

        jogAudioSource = GameObject.Find("JogAudio").GetComponent<AudioSource>();
        jumpAudioSource = GameObject.Find("JumpAudio").GetComponent<AudioSource>();
        placeAudioSource = GameObject.Find("PlaceAudio").GetComponent<AudioSource>();
        buffAudioSource = GameObject.Find("BuffAudio").GetComponent<AudioSource>();

        playerAnimator.SetBool("isJogging", false);
        playerAnimator.SetBool("isPlacing", false);
        playerAnimator.SetBool("isBuffing", false);
        playerAnimator.SetBool("isPunching", false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector3.zero)
        {
            jogAudioSource.Play();
            if (!playerAnimator.GetBool("isJogging"))
            {
                playerAnimator.SetBool("isJogging", true);
            }

            Debug.Log("isJogging set to " + playerAnimator.GetBool("isJogging"));
        }
        else
        {
            jogAudioSource.Stop();
            playerAnimator.SetBool("isJogging", false);
            Debug.Log("isJogging set to " + playerAnimator.GetBool("isJogging"));
        }
        // Debug.Log($"Move Input: {moveInput}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            //Debug.Log("We are supposed to jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpAudioSource.Play();
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speed = baseSpeed * multiplier;
        //Debug.Log($"Speed set to: {speed}");
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
        //Debug.Log($"Speed reset to: {speed}");
    }

    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        controller.Move(moveDirection * speed * Time.deltaTime);

        if (shouldFaceMoveDirection && moveDirection != Vector3.zero)
        {
            //Debug.Log("Rotating towards: " + moveDirection);
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (
            Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Alpha3) |
            Input.GetKeyDown(KeyCode.Alpha4) |
            Input.GetKeyDown(KeyCode.Alpha5) |
            Input.GetKeyDown(KeyCode.Keypad1) | Input.GetKeyDown(KeyCode.Keypad2) |
            Input.GetKeyDown(KeyCode.Keypad3) |
            Input.GetKeyDown(KeyCode.Keypad4) |
            Input.GetKeyDown(KeyCode.Keypad5)
        )
        {
            placeAudioSource.Play();
            playerAnimator.SetBool("isPlacing", true);
            Debug.Log("isPlacing set to " + playerAnimator.GetBool("isPlacing"));
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            buffAudioSource.Play();
            playerAnimator.SetBool("isBuffing", true);
            Debug.Log("isBuffing set to " + playerAnimator.GetBool("isBuffing"));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpAudioSource.Play();
            playerAnimator.SetBool("isJumping", true);
            playerAnimator.SetBool("isJogging", false);
            Debug.Log("isJumping set to " + playerAnimator.GetBool("isJumping"));
        }
        else if (Input.GetMouseButtonDown(0) | Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetBool("isPunching", true);
        }
        else
        {
            playerAnimator.SetBool("isBuffing", false);
            playerAnimator.SetBool("isPlacing", false);
            playerAnimator.SetBool("isPunching", false);
            // Debug.Log("isPlacing set to " + playerAnimator.GetBool("isPlacing"));
            // Debug.Log("isBuffing set to " + playerAnimator.GetBool("isBuffing"));
        }
    }
}