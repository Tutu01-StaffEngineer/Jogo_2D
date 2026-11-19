using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScene : MonoBehaviour
{
    public GameObject painelPause;
    public GameObject painelGameOver;
    public GameObject PainelVitoria;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Portal"))
            {
                PassarFase();
            }

            if (gameObject.CompareTag("BlocoVitoria"))
            {
                MostrarVitoria();
            }
        }
    }
    public void PassarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
   

    public void MostrarVitoria()
    {
        Time.timeScale = 0f;
        PainelVitoria.SetActive(true);

    }
}   

 

