using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StackSceneManager : MonoBehaviour
{ // Instância do Singleton para você conseguir chamar de qualquer outro script
    public static StackSceneManager Instance { get; private set; }
    [Header("Configuração do Pause")]
    [SerializeField] private string nomeCenaPause = "MenuPause";

    private Stack<string> sceneStack = new Stack<string>();
    private bool jogoPausado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sceneStack.Push(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // O Update fica checando o teclado a cada frame do jogo
    private void Update()
    {
        // Detecta se o jogador apertou ESC (ou a tecla 'P' se preferir mudar)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPause();
        }
    }

    // Função que decide se abre ou fecha o pause automaticamente
    public void AlternarPause()
    {
        if (!jogoPausado)
        {
            PushScene(nomeCenaPause);
        }
        else
        {
            PopScene();
        }
    }

    public void LoadNewScene(string sceneName)
    {
        Time.timeScale = 1f;
        jogoPausado = false; // Reseta o estado do pause para a nova fase
        sceneStack.Clear();
        sceneStack.Push(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void PushScene(string menuSceneName)
    {
        Time.timeScale = 0f;
        jogoPausado = true; // Marca que o jogo está pausado

        sceneStack.Push(menuSceneName);
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Additive);
    }

    public void PopScene()
    {
        if (sceneStack.Count > 1)
        {
            string currentScene = sceneStack.Pop();
            SceneManager.UnloadSceneAsync(currentScene);

            Time.timeScale = 1f;
            jogoPausado = false; // Marca que o jogo voltou ao normal
        }


    }
}
