using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [Header("Input System")]
    public InputActionReference moveAction;      // Vector2
    public InputActionReference interactAction;  // Button (F)
    public InputActionReference clickAction;     // Button (Mouse Left)
    public InputActionReference smokeAction;     // Button (Q)

    public Vector2 Move { get; private set; }
    public Vector2 Facing { get; private set; } = Vector2.down;

    bool _interactPressed;
    bool _clickPressed;

    // --- Smoke ---
    bool _smokePressedEdge;            // flanco: se prendió este frame (consume)
    public bool SmokeHeld { get; private set; } // está mantenida

    void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.performed += OnMove;
            moveAction.action.canceled  += OnMove;
            moveAction.action.Enable();
        }

        if (interactAction != null)
        {
            interactAction.action.performed += OnInteract;
            interactAction.action.Enable();
        }

        if (clickAction != null)
        {
            clickAction.action.performed += OnClick;
            clickAction.action.Enable();
        }

        if (smokeAction != null)
        {
            // ¡enganchamos a OnSmoke!
            smokeAction.action.performed += OnSmokePerformed;
            smokeAction.action.canceled  += OnSmokeCanceled;
            smokeAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.performed -= OnMove;
            moveAction.action.canceled  -= OnMove;
            moveAction.action.Disable();
        }

        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
        }

        if (clickAction != null)
        {
            clickAction.action.performed -= OnClick;
            clickAction.action.Disable();
        }

        if (smokeAction != null)
        {
            smokeAction.action.performed -= OnSmokePerformed;
            smokeAction.action.canceled  -= OnSmokeCanceled;
            smokeAction.action.Disable();
        }
    }

    void Update()
    {
        if (Move.sqrMagnitude > 0.0001f)
            Facing = Move.normalized;
    }

    void OnMove(InputAction.CallbackContext ctx)
        => Move = ctx.ReadValue<Vector2>();

    void OnInteract(InputAction.CallbackContext ctx)
        => _interactPressed = true;

    void OnClick(InputAction.CallbackContext ctx)
        => _clickPressed = true;

    // --- Smoke handlers ---
    void OnSmokePerformed(InputAction.CallbackContext ctx)
    {
        // flanco (una sola vez)
        _smokePressedEdge = true;
        // estado mantenido
        SmokeHeld = true;
        // Debug.Log("Smoke down");
    }
    void OnSmokeCanceled(InputAction.CallbackContext ctx)
    {
        SmokeHeld = false;
        // Debug.Log("Smoke up");
    }

    public bool HasMovementInput(float deadZone = 0.01f)
        => Move.sqrMagnitude > deadZone * deadZone;

    public bool ConsumeInteractPressed()
    {
        if (_interactPressed) { _interactPressed = false; return true; }
        return false;
    }

    public bool ConsumeClickPressed()
    {
        if (_clickPressed) { _clickPressed = false; return true; }
        return false;
    }

    // --- API para fumar ---
    /// Devuelve true una sola vez cuando se apretó Q.
    public bool ConsumeSmokePressed()
    {
        if (_smokePressedEdge) { _smokePressedEdge = false; return true; }
        return false;
    }

    /// Devuelve si Q está mantenida.
    public bool IsSmokingHeld() => SmokeHeld;

    // Click a world (te lo dejo igual que lo tenías)
    public bool TryConsumeClickWorldPos(Camera cam, out Vector2 worldPos)
    {
        worldPos = default;
        if (!ConsumeClickPressed()) return false;
        var mouse = Mouse.current;
        if (mouse == null) return false;
        Vector2 screen = mouse.position.ReadValue();
        worldPos = cam.ScreenToWorldPoint(screen);
        return true;
    }
}
