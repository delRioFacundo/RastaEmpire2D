using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    [Range(0f, 1f)] public float acceleration = 0.25f;
    [Range(0f, 1f)] public float deceleration = 0.15f;

    Rigidbody2D _rb;
    Vector2 _currentVelocity; // smoothed

    void Awake() => _rb = GetComponent<Rigidbody2D>();

    /// Llamado por el estado de movimiento
    public void Move(Vector2 rawInput)
    {
        // Normaliza para que la diagonal no sea más rápida
        Vector2 target = rawInput.sqrMagnitude > 1f ? rawInput.normalized : rawInput;

        // Acelera / desacelera
        float lerp = target.sqrMagnitude > 0.001f ? acceleration : deceleration;
        _currentVelocity = Vector2.Lerp(_currentVelocity, target * moveSpeed, lerp);

        // Aplicar movimiento
        _rb.MovePosition(_rb.position + _currentVelocity * Time.fixedDeltaTime);
    }

    /// Útil para cortar en seco al entrar a Idle
    public void StopInstant()
    {
        _currentVelocity = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }
}
