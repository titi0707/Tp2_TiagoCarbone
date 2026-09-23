using UnityEngine;

public class DistractionInventory : MonoBehaviour
{
    [SerializeField] private int cantidadInicial = 0;
    private int cantidadActual;

    public int CantidadActual => cantidadActual;

    private void Awake()
    {
        cantidadActual = cantidadInicial;
    }

    public void AgregarObjeto(int cantidad = 1)
    {
        cantidadActual += cantidad;
    }

    public bool TieneObjetos()
    {
        return cantidadActual > 0;
    }

    public void UsarObjeto()
    {
        if (cantidadActual > 0)
            cantidadActual--;
    }
}