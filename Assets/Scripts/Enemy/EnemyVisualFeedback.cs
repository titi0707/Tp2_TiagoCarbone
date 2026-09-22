using UnityEngine;

public class EnemyVisualFeedback : MonoBehaviour
{
   
    [SerializeField] private EnemyStateController controladorEstado;
    [SerializeField] private Renderer rendererEnemigo; 

    
    [SerializeField] private Color colorPatrullando = Color.green;
    [SerializeField] private Color colorAlerta = Color.yellow;
    [SerializeField] private Color colorPersiguiendo = Color.red;

    private Material material;

    private void Awake()
    {
        material = rendererEnemigo.material; 
    }

    private void Update()
    {
        switch (controladorEstado.EstadoActual)
        {
            case EnemyStateController.Estado.Patrullando:
                material.color = colorPatrullando;
                break;
            case EnemyStateController.Estado.Alerta:
                material.color = colorAlerta;
                break;
            case EnemyStateController.Estado.Persiguiendo:
                material.color = colorPersiguiendo;
                break;
        }
    }
}