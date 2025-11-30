using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControllerMouse : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;          // speed for WASD
    public float mouseSensitivity = 3f;    // how fast the ship rotates with mouse

    [Header("Water")]
    public float waterHeight = 0f;         // Y level of water
    public bool lockToWaterHeight = true;  // keep ship on water Y

    private Rigidbody rb;
    private float yaw;                     // current rotation around Y

    private Vector3 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;         // we will control rotation ourselves
    }

    void Start()
    {
        yaw = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;   // optional
        Cursor.visible = false;                     // optional
    }

    void Update()
    {
        // --- Keyboard movement (no rotation) ---
        float h = Input.GetAxisRaw("Horizontal");   // A/D  -> left/right
        float v = Input.GetAxisRaw("Vertical");     // W/S  -> forward/back

        // movement in local space (relative to ship’s facing)
        moveInput = (transform.forward * v + transform.right * h).normalized;

        // --- Mouse rotation (yaw only) ---
        float mouseX = Input.GetAxis("Mouse X");
        yaw += mouseX * mouseSensitivity;
    }

    void FixedUpdate()
    {
        // Apply rotation from mouse
        Quaternion targetRot = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(targetRot);

        // Apply movement from WASD
        Vector3 targetPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // Keep ship on water level if desired
        if (lockToWaterHeight)
            targetPos.y = waterHeight;

        rb.MovePosition(targetPos);
    }
}
