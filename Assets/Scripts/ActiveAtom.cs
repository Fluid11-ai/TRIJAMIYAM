using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ActiveAtom : MonoBehaviour
{
    [Header("Movement")]
    public float basePower = 2f;

    [Header("Frequency (Discrete)")]
    public Slider resonanceSlider; // 0–10, whole numbers

    [Header("Trajectory")]
    public LineRenderer lineRenderer;
    public int linePoints = 10;
    public float lineStep = 0.15f;

    private Rigidbody rb;
    private Camera cam;

    private bool isAiming;
    private bool directionLocked;
    private bool hasStabilized;

    private Vector3 shootDirection;
    private Vector3 worldDragStart;
    private WaitForSeconds fireCooldownWait;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        fireCooldownWait = new WaitForSeconds(0.5f);

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.isKinematic = false;

        resonanceSlider.value = 0;
        resonanceSlider.wholeNumbers = true;
    }

    void Update()
    {
        if (hasStabilized) return;

        HandleAiming();
        HandleFireInput();
    }

    // ===============================
    // UTILITY
    // ===============================
    Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(cam.transform.position.z);
        return cam.ScreenToWorldPoint(screenPos);
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    // ===============================
    // AIMING
    // ===============================
    void HandleAiming()
    {
        if (directionLocked) return;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            isAiming = true;
            worldDragStart = GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(0) && isAiming)
        {
            DrawDirectionPreview();
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            LockDirection();
            isAiming = false;
        }
    }

    void LockDirection()
    {
        Vector3 dir = worldDragStart - GetMouseWorldPosition();
        if (dir.magnitude < 0.1f) return;

        shootDirection = dir.normalized;
        directionLocked = true;
        ClearTrajectory();
    }

    // ===============================
    // FIRE (RELIABLE)
    // ===============================
    void HandleFireInput()
    {
        if (!directionLocked) return;

        // Second tap anywhere fires
        if (Input.GetMouseButtonDown(0))
        {
            int frequency = (int)resonanceSlider.value;
            if (frequency <= 0) return;

            FireAtom(frequency);
            ResetState();
            StartCoroutine(FireCooldown());
        }
    }

    IEnumerator FireCooldown()
    {
        directionLocked = true;
        yield return fireCooldownWait;
        directionLocked = false;
    }

    void FireAtom(int frequency)
    {
        float finalPower = basePower * frequency;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(shootDirection * finalPower, ForceMode.Impulse);

        Debug.Log($"Fired | Frequency: {frequency}, Power: {finalPower}");
    }

    void ResetState()
    {
        resonanceSlider.value = 0;
        ClearTrajectory();
    }

    // ===============================
    // VISUALS
    // ===============================
    void DrawDirectionPreview()
    {
        Vector3 dir = (worldDragStart - GetMouseWorldPosition()).normalized;
        DrawLine(dir, basePower);
    }

    void DrawLine(Vector3 dir, float power)
    {
        lineRenderer.positionCount = linePoints;
        Vector3 start = transform.position;

        for (int i = 0; i < linePoints; i++)
        {
            float t = i * lineStep;
            lineRenderer.SetPosition(i, start + dir * t * power);
        }
    }

    void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    // ===============================
    // SUCCESS
    // ===============================
    void OnTriggerEnter(Collider other)
    {
        if (hasStabilized) return;

        if (other.CompareTag("Goal"))
        {
            hasStabilized = true;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            transform.position = other.transform.position;

            GameManager.Instance.LevelComplete();
        }
    }

}
