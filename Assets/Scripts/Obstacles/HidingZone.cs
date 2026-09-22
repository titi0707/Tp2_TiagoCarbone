using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HidingZone : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHidingStatus estado = other.GetComponent<PlayerHidingStatus>();
        if (estado != null)
            estado.EstaEscondido = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHidingStatus estado = other.GetComponent<PlayerHidingStatus>();
        if (estado != null)
            estado.EstaEscondido = false;
    }
}