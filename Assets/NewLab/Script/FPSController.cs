using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;
    public bool LockMovement = false;

    [Header("Camera Look")]
    public Transform cameraTransform; // Kéo Camera vào đây
    public float mouseSensitivity = 2.0f;

    [Header("Zoom Settings")]
    public float zoomFactor = 1.5f;
    public float zoomSpeed = 10f;

    private CharacterController characterController;
    private float verticalRotation = 0f;
    private Vector3 velocity;
    
    // Zoom Variables
    private Camera playerCamera;
    private float originalFOV;
    private float targetFOV;

    // Singleton Pattern (giữ nguyên)
    private static FPSController instance;
    public static FPSController Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null) { instance = this; }
        if (instance.GetInstanceID() != GetInstanceID()) { Destroy(gameObject); }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // --- KHỞI TẠO BIẾN ZOOM ---
        // Lấy component Camera từ transform đã có
        playerCamera = cameraTransform.GetComponent<Camera>();
        if (playerCamera != null)
        {
            originalFOV = playerCamera.fieldOfView;
            targetFOV = originalFOV;
        }
    }

    void Update()
    {
        // Xử lý di chuyển và xoay camera
        if (!LockMovement)
        {
            HandleRotation();
            HandleMovement();
        }

        // Xử lý trọng lực luôn hoạt động
        ApplyGravity();

        // Xử lý zoom
        HandleZoom();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // --- HÀM MỚI ĐỂ XỬ LÝ ZOOM ---
    void HandleZoom()
    {
        if (playerCamera == null) return;

        if (Input.GetMouseButton(1)) // 1 là chuột phải
        {
            targetFOV = originalFOV / zoomFactor;
        }
        else
        {
            targetFOV = originalFOV;
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}