using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerBehavior : MonoBehaviour
{
    // ---------- References ----------
    [Header("References")]
    public CharacterController controller;         // on Player
    public Transform yawRoot;                    // drag Player here (the object that rotates left/right)
    public Transform pitchCamera;                // drag Main Camera here (tilts up/down only)
    public AudioSource footstepSource;           // ONE AudioSource on Player (no loop)

    // ---------- Mouse Look ----------
    [Header("Mouse Look")]
    public float mouseSensitivity = 150f;
    public float minPitch = -80f;
    public float maxPitch = 80f;
    public bool invertLookY = false;
    public bool invertLookX = false;
    float pitch;

    // ---------- Movement ----------
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintMultiplier = 1.6f;
    public bool moveRelativeToCamera = true;
    bool wasMoving;
    float currentHorizSpeed;
    public float moveSpeedThreshold = 0.1f;


    // ---------- Jump / Gravity ----------
    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;
    Vector3 verticalVel;

    // ---------- Crouch ----------
    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public bool crouchHold = true;              // true = hold, false = toggle
    public float standHeight = 0.8f;
    public float crouchHeight = 1.1f;
    public float crouchSpeedMultiplier = 0.55f;
    public float heightLerpSpeed = 10f;
    bool isCrouching;
    float baseCamLocalY;

    // ---------- Head Bob ----------
    [Header("Head Bob")]
    public float bobWalkFrequency = 7.5f;
    public float bobWalkAmplitude = 0.035f;
    public float bobSprintFrequency = 10.5f;
    public float bobSprintAmplitude = 0.055f;
    public float bobCrouchFrequency = 6.0f;
    public float bobCrouchAmplitude = 0.02f;

    // ---------- Footsteps (SINGLE sound) ----------
    [Header("Footsteps")]
    // The single AudioClip assigned directly to the AudioSource component will be used now.

    public float stepIntervalWalk = 0.5f;
    public float stepIntervalSprint = 0.36f;
    public float stepIntervalCrouch = 0.65f;

    [Tooltip("Random pitch range to avoid repetition (keep close to 1).")]
    public Vector2 pitchJitter = new Vector2(0.97f, 1.03f);

    float stepTimer;

    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
        if (!yawRoot) yawRoot = transform;

        // Try to auto-assign the pitch camera if not set
        if (!pitchCamera)
        {
            var cam = Camera.main;
            if (cam != null)
            {
                pitchCamera = cam.transform;
            }
            else
            {
                Debug.LogError("PlayerBehavior: No Main Camera found and Pitch Camera not assigned.");
            }
        }

        if (!footstepSource) footstepSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pitchCamera) baseCamLocalY = pitchCamera.localPosition.y;

        // Ensure controller starts at stand height
        controller.height = standHeight;
        //controller.center = new Vector3(0f, 0, 0f);

        // Make the footstep source sensible
        if (footstepSource)
        {
            footstepSource.loop = false;
            footstepSource.playOnAwake = false;
            // Set 3D spatial blend for realism, but 0 is fine for testing
            footstepSource.spatialBlend = 1f;
        }
    }
    

    void Update()
    {
        HandleLook();
        // movingGrounded and sprinting are calculated here and passed to other methods
        HandleMoveJump(out bool movingGrounded, out bool sprinting);
       // HandleCrouchAndHeadBob(movingGrounded, sprinting);
        // Pass the already calculated movement states
        HandleFootsteps(movingGrounded, sprinting);
    }

    // ---------------- LOOK ----------------
    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        mouseX *= invertLookX ? -1f : 1f;
        mouseY *= invertLookY ? 1f : -1f;

        // Pitch (up/down) on the camera
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        if (pitchCamera) pitchCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Yaw (left/right) on the body/root
        if (yawRoot) yawRoot.Rotate(0f, mouseX, 0f, Space.Self);
    }

    // ------------- MOVE / JUMP / GRAVITY -------------
    void HandleMoveJump(out bool movingGrounded, out bool sprinting)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right, fwd;
        if (moveRelativeToCamera && pitchCamera != null)
        {
            Vector3 camFwd = pitchCamera.forward; camFwd.y = 0f; camFwd.Normalize();
            fwd = camFwd;
            right = new Vector3(fwd.z, 0f, -fwd.x);
        }
        else
        {
            right = yawRoot.right;
            fwd = yawRoot.forward;
        }

        Vector3 inputDir = (right * x + fwd * z).normalized;

        sprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && inputDir.sqrMagnitude > 0.01f;
        float speed = walkSpeed * (sprinting ? sprintMultiplier : 1f) * (isCrouching ? crouchSpeedMultiplier : 1f);

        // Horizontal move
        controller.Move(inputDir * speed * Time.deltaTime);

        // Ground handling
        bool grounded = controller.isGrounded;
        if (grounded && verticalVel.y < 0f) verticalVel.y = -2f;

        // Jump (disabled when crouched)
        if (grounded && Input.GetButtonDown("Jump") && !isCrouching)
            verticalVel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        verticalVel.y += gravity * Time.deltaTime;
        controller.Move(verticalVel * Time.deltaTime);

        currentHorizSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        movingGrounded = grounded && currentHorizSpeed > moveSpeedThreshold;
    }

    // ------------- CROUCH + HEAD-BOB -------------
    //void HandleCrouchAndHeadBob(bool movingGrounded, bool sprinting)
    //{
    //    if (crouchHold)
    //        isCrouching = Input.GetKey(crouchKey);
    //    else if (Input.GetKeyDown(crouchKey))
    //        isCrouching = !isCrouching;

    //    float targetH = isCrouching ? crouchHeight : standHeight;
    //    controller.height = Mathf.Lerp(controller.height, targetH, Time.deltaTime * heightLerpSpeed);
    //    controller.center = new Vector3(0f, controller.height * 0.5f, 0f);

    //    if (!pitchCamera) return;

    //    float freq = bobWalkFrequency, amp = bobWalkAmplitude;
    //    if (sprinting) { freq = bobSprintFrequency; amp = bobSprintAmplitude; }
    //    else if (isCrouching) { freq = bobCrouchFrequency; amp = bobCrouchAmplitude; }

    //    float bob = movingGrounded ? Mathf.Sin(Time.time * freq) * amp : 0f;

    //    float crouchBase = baseCamLocalY - (standHeight - controller.height) * 0.5f;
    //    Vector3 camLocal = pitchCamera.localPosition;
    //    camLocal.y = Mathf.Lerp(camLocal.y, crouchBase + bob, Time.deltaTime * 12f);
    //    pitchCamera.localPosition = camLocal;
    //}

    // ------------- FOOTSTEPS (single sound) -------------
    void HandleFootsteps(bool movingGrounded, bool sprinting)
    {
        // Check if we have the necessary components and a clip assigned
        if (!controller || !footstepSource || footstepSource.clip == null)
        {
            stepTimer = 0f;
            wasMoving = false;
            return;
        }

        // Fire one step instantly when movement starts
        if (movingGrounded && !wasMoving)
        {
            PlayStepNow();
            stepTimer = 0f;                                    // start cadence fresh
        }

        // Stop counting when we stop or leave ground
        if (!movingGrounded)
        {
            stepTimer = 0f;
            wasMoving = false;
            return;
        }

        // Choose cadence by state
        float interval =
            isCrouching ? stepIntervalCrouch :
            sprinting ? stepIntervalSprint :
                        stepIntervalWalk;

        // Tick and fire
        stepTimer += Time.deltaTime;
        if (stepTimer >= interval)
        {
            stepTimer = stepTimer % interval; // Use modulo to keep remainder for next frame
            PlayStepNow();
        }

        wasMoving = true;
    }

    // Reverted to use the single clip assigned to the AudioSource
    void PlayStepNow()
    {
        // Redundant check, but safe
        if (!footstepSource || footstepSource.clip == null) return;

        // 1. Set pitch jitter
        footstepSource.pitch = Random.Range(pitchJitter.x, pitchJitter.y);

        // 2. Play the clip assigned to the AudioSource component
        footstepSource.PlayOneShot(footstepSource.clip);
    }
}
