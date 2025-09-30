using UnityEngine;
public interface IInteractable
{
    /// Acción a ejecutar cuando el jugador interactúa (ej: abrir UI, sumar item, dialogar, etc.)
    void Interact(Player player);

    /// Punto opcional para alinear al jugador (si querés “pegarlo” a un lugar antes de interactuar)
    Transform InteractionAnchor { get; }

    /// Texto opcional para la UI de ayuda (ej: "Presioná F")
    string Prompt { get; }
}
