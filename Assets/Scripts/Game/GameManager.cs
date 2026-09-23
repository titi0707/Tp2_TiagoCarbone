using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    private bool juegoTerminado = false;

    private void Awake()
    {
        Instancia = this;
    }

    private void Update()
    {
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarNivel();
        }
    }

    public void PerderJuego()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        Time.timeScale = 0f;
        Debug.Log("PERDISTE - Te atrapó el enemigo. Presioná R para reiniciar.");
    }

    private void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}