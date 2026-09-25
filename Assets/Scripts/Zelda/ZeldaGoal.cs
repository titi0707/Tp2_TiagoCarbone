using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZeldaGoal : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instancia.GanarJuego();
    }
}