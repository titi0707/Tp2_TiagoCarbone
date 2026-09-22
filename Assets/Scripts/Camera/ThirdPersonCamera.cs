using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
   
    [SerializeField] private Transform jugador;

    
    [SerializeField] private Vector3 offsetLocal = new Vector3(0f, 3f, -5f); 
    [SerializeField] private float suavizadoPosicion = 5f;
    [SerializeField] private float suavizadoRotacion = 5f;

    private void LateUpdate()
    {
        if (jugador == null) return;

       
        Vector3 offsetRotado = jugador.rotation * offsetLocal;
        Vector3 posicionObjetivo = jugador.position + offsetRotado;

        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizadoPosicion * Time.deltaTime);

        Quaternion rotacionObjetivo = Quaternion.LookRotation(jugador.position + Vector3.up * 1.5f - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
    }
}