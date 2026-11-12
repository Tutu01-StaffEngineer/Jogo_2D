using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScene : MonoBehaviour
{
    public GameObject painelPause;
    public GameObject painelGameOver;

    bool pausado = false;
    bool jogoAcabou = false;
    void Start()
    {
        painelPause.SetActive(false); //ativar ou desativar o PAUSE
        painelGameOver.SetActive(false); //ativar ou desativar meu GameOver
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        if (jogoAcabou == false)
        {
            pausado = true;
            painelPause.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Continuar()
    {
        pausado = false;
        painelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MostrarGameOver()
    {
        jogoAcabou = true;
        painelGameOver.SetActive(true);
        Time.timeScale = 0f;

    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PassarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("COLISAO DETECTADA!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTADO!");
            PassarFase();
        }
    }


}

 

