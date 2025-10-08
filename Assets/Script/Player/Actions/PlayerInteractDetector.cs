using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteractDetector : MonoBehaviour
{
    public LayerMask interactableMask;
    public float probeRadius = 0.5f;
    public float probeDistance = 0.75f;
    public float facingDeadZone = 0.2f;

    Player _player;
    IInteractable _lastBest = null;
    void Awake() => _player = GetComponent<Player>();

    public IInteractable FindBest()
    {
        Vector2 facing = _player.input != null ? _player.input.Facing : Vector2.down;
        if (facing.sqrMagnitude < facingDeadZone * facingDeadZone)
            facing = Vector2.down;

        Vector2 origin = (Vector2)transform.position + facing * probeDistance;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, probeRadius, interactableMask);
        if (hits == null || hits.Length == 0) return null;

        float best = float.MaxValue;
        IInteractable pick = null;

        foreach (var h in hits)
        {
            var ix = h.GetComponentInParent<IInteractable>();
            if (ix == null) continue;

            float d = (h.bounds.center - (Vector3)origin).sqrMagnitude;
            if (d < best) { best = d; pick = ix; }
        }
        return pick;
    }

    public IInteractable FindBestAndShowUI()
    {
        Vector2 origin = (Vector2)transform.position + _player.input.Facing * probeDistance;
        var hits = Physics2D.OverlapCircleAll(origin, probeRadius, interactableMask);

        IInteractable best = null;
        float bestD = float.MaxValue;

        foreach (var h in hits)
        {
            var ix = h.GetComponent<IInteractable>() ?? h.GetComponentInParent<IInteractable>();
            if (ix == null) continue;

            float d = ((Vector2)h.bounds.center - origin).sqrMagnitude;
            if (d < bestD)
            {
                bestD = d;
                best = ix;
            }
        }

        if (best != _lastBest)
        {
            if (best != null)
            {
                InteractionUI.Instance.Show(best.Prompt);
                EventsManagerSpaar.TriggerEvent(EventTypeSpaar.OpenPopup, "Interaction");
                print("best es " + best);

                if (best is PlantInteractable pi && pi.plant != null)
                {
                    EventsManagerSpaar.TriggerEvent(EventTypeSpaar.PlantDetected, pi.plant);
                    print("Disparando evento de planta detectada");
                }


                // si es planta, guardarla
                _player.CurrentPlant = best as PlantInteractable;
            }
            else
            {
                InteractionUI.Instance.Hide();
                EventsManagerSpaar.TriggerEvent(EventTypeSpaar.ClosePopup, "Interaction");

                // Si saliste de rango, limpiá la referencia
                _player.CurrentPlant = null;
            }
        }

        _lastBest = best;
        return best;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var facing = Application.isPlaying && _player && _player.input ? _player.input.Facing : Vector2.down;
        Vector2 origin = (Vector2)transform.position + facing * probeDistance;
        Gizmos.DrawWireSphere(origin, probeRadius);
    }
}
