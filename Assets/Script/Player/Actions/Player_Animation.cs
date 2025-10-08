using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    public Player player;

    Animator _anim;
    SpriteRenderer _sr;

    enum Dir { Down = 0, Up = 1, Side = 2 }

    [Header("Filtros de dirección")]
    [Tooltip("Ventana para no caer a Idle entre cambios (segundos)")]
    public float stickyMoveWindow = 0.04f;   // más ágil

    [Tooltip("Tiempo que hay que sostener una nueva dirección para confirmarla")]
    public float holdToTurn = 0.05f;         // más ágil

    [Tooltip("Margen que debe superar el nuevo eje para cambiar (histeresis)")]
    public float axisHysteresis = 0.10f;     // más sensible

    [Tooltip("Suavizado del facing para evitar saltos bruscos")]
    public float facingLerp = 20f;           // giro visual más rápido

    [Header("Snap inmediato en giros claros")]
    [Tooltip("Si el nuevo eje domina por este margen, se cambia de dirección YA")]
    public float snapMargin = 0.25f;         // 0.20–0.30 va bien

    Vector2 _lastFacing = Vector2.down;
    float _lastNonZeroTime;


    Dir _currentDir = Dir.Down;
    Dir _candidateDir = Dir.Down;
    float _candidateStartTime;

    bool _locomotionEnabled;

    // === PUBLIC API: lectura de la dirección/facing actual ===
    public int GetCurrentDir() => (int)_currentDir;          // 0=Down, 1=Up, 2=Side
    public Vector2 GetLastFacing() => _lastFacing;
    public bool IsFacingLeftOnSide() => (_currentDir == Dir.Side && _lastFacing.x < 0f);


    void Awake()
    {
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        if (!player) player = GetComponentInParent<Player>();
    }

    // --- API pública ---------------------------------------------------------

    /// Llamar desde State_Move.Enter/Exit
    public void EnableLocomotion(bool enabled)
    {
        _locomotionEnabled = enabled;

        if (!enabled)
        {
            // resetear “sticky” y asegurar Idle
            _lastNonZeroTime = -999f;
            _anim.SetBool("isMoving", false);
        }
    }

    /// Llamar SOLO desde State_Move.UpdateState()
    public void TickLocomotion(Vector2 rawMove)
    {
        if (!_locomotionEnabled) return;

        bool hasRaw = rawMove.sqrMagnitude > 0.0001f;
        if (hasRaw) _lastNonZeroTime = Time.time;

        // seguimos “en movimiento” un ratito aunque raw sea 0
        bool isMoving = (Time.time - _lastNonZeroTime) <= stickyMoveWindow;

        // facing suavizado (solo para flip/idle look)
        Vector2 targetFacing = hasRaw ? rawMove.normalized : _lastFacing;
        _lastFacing = Vector2.Lerp(
            _lastFacing,
            targetFacing,
            1f - Mathf.Exp(-facingLerp * Time.deltaTime)
        );
        if (_lastFacing.sqrMagnitude < 0.0001f) _lastFacing = targetFacing;

        // dirección propuesta (con histeresis)
        Dir proposed = ChooseDirWithHisteresis(rawMove);

        // SNAP inmediato si el giro es claro (no esperamos holdToTurn)
        if (IsStrongTurn(rawMove, _currentDir, snapMargin))
        {
            _currentDir = ChooseDirImmediate(rawMove);
            _candidateDir = _currentDir;
            _candidateStartTime = Time.time;
        }
        else
        {
            // si no fue un giro claro, aplicar hold/histeresis
            UpdateDirWithHoldToTurn(proposed);
        }

        ApplyFlipAndParams(isMoving, _currentDir, _lastFacing);
    }

    /// Para estados NO-movimiento (Idle, Interact, Smoke, etc.)
    public void ForceIdleFacing(Vector2 facing)
    {
        _locomotionEnabled = false;
        _lastFacing = (facing.sqrMagnitude > 0.0001f) ? facing.normalized : _lastFacing;

        Dir d = ChooseDirSimple(_lastFacing);
        _currentDir = d;
        _candidateDir = d;
        _candidateStartTime = Time.time;

        ApplyFlipAndParams(false, d, _lastFacing); // isMoving = false
    }

    // --- Helpers internos ----------------------------------------------------

    void ApplyFlipAndParams(bool isMoving, Dir dir, Vector2 facing)
    {
        _sr.flipX = (dir == Dir.Side && facing.x < 0f);
        _anim.SetBool("isMoving", isMoving);
        _anim.SetInteger("dir", (int)dir);
    }

    void UpdateDirWithHoldToTurn(Dir proposed)
    {
        if (proposed != _currentDir)
        {
            if (proposed != _candidateDir)
            {
                _candidateDir = proposed;
                _candidateStartTime = Time.time;
            }
            else if (Time.time - _candidateStartTime >= holdToTurn)
            {
                _currentDir = _candidateDir;
            }
        }
        else
        {
            _candidateDir = _currentDir;
            _candidateStartTime = Time.time;
        }
    }

    // Giro fuerte: el nuevo eje domina por 'margin' → cambio inmediato
    bool IsStrongTurn(Vector2 raw, Dir current, float margin)
    {
        float ax = Mathf.Abs(raw.x);
        float ay = Mathf.Abs(raw.y);
        if (ax < 0.0001f && ay < 0.0001f) return false;

        if (current == Dir.Side) return ay > ax + margin; // lateral -> vertical claro
        if (current == Dir.Up || current == Dir.Down) return ax > ay + margin; // vertical -> lateral claro
        return false;
    }

    // Dirección instantánea por eje dominante (sin histeresis)
    Dir ChooseDirImmediate(Vector2 v)
    {
        return (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            ? Dir.Side
            : (v.y >= 0f ? Dir.Up : Dir.Down);
    }

    // Histeresis eje dominante
    Dir ChooseDirWithHisteresis(Vector2 v)
    {
        float ax = Mathf.Abs(v.x);
        float ay = Mathf.Abs(v.y);
        if (ax < 0.0001f && ay < 0.0001f) return _currentDir;

        if (ax > ay + axisHysteresis) return Dir.Side;
        if (ay > ax + axisHysteresis) return (v.y >= 0f) ? Dir.Up : Dir.Down;

        // zona muerta: mantener
        switch (_currentDir)
        {
            case Dir.Side: return Dir.Side;
            case Dir.Up: return (v.y >= 0f) ? Dir.Up : Dir.Down;
            case Dir.Down: return (v.y >= 0f) ? Dir.Up : Dir.Down;
            default: return Dir.Side;
        }
    }

    // Versión simple para forzar idle/facing
    Dir ChooseDirSimple(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y)) return Dir.Side;
        return v.y >= 0f ? Dir.Up : Dir.Down;
    }

    public void SetWatering(bool value)
    {
        _anim.SetBool("isWatering", value);
    }


    // Este método será llamado desde el evento de la animación
    public void OnWaterAnimationEnd()
    {
        // Apagar el bool
        _anim.SetBool("isWatering", false);

        // Opcional: forzar a Idle en la última dirección
        ApplyFlipAndParams(false, _currentDir, _lastFacing);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game");
        player.plant.Water();
    }


    public void SetFertilizing(bool value)
    {
        _anim.SetBool("isFertilizing", value);
    }
    // Este método será llamado desde el evento de la animación
    public void OnFertilizeAnimationEnd()
    {
        // Apagar el bool
        _anim.SetBool("isFertilizing", false);

        // Opcional: forzar a Idle en la última dirección
        ApplyFlipAndParams(false, _currentDir, _lastFacing);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game");
        player.plant.Fertilize();
    }


    public void SetProtectionSpray(bool value)
    {
        _anim.SetBool("isProtectionSpray", value);
    }
    // Este método será llamado desde el evento de la animación
    public void OnProtectionSprayAnimationEnd()
    {
        // Apagar el bool
        _anim.SetBool("isProtectionSpray", false);

        // Opcional: forzar a Idle en la última dirección
        ApplyFlipAndParams(false, _currentDir, _lastFacing);
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ChangeStatePlayer, "Idle");
        EventsManagerSpaar.TriggerEvent(EventTypeSpaar.CambiarPanel, "Panel Game");
        player.plant.ProtectionSpray();
    }

}
