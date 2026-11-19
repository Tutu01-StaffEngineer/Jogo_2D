using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // Permite utilizar o Slider da barra de vida.
public class Player : MonoBehaviour
{
    public Slider barraVida; // Referência à barra de vida da interface.
    public float vidaMaxima = 100f; // Define a vida máxima do jogador.
    public float danoInimigo = 20f; // Define quanto de vida o inimigo retira.
    private float vidaAtual; // Armazena a vida atual do jogador.

    public float speed = 5f;

    // Configurações do Dash
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public MenuScene menu;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDashing = false;
    private bool canDash = true;


    // Guarda a última direção em que o jogador andou
    private float lastDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do jogador.

        vidaAtual = vidaMaxima; // Começa com a vida cheia.

        barraVida.maxValue = vidaMaxima; // Define o máximo da barra como 100.

        barraVida.value = vidaAtual; // Preenche a barra completamente.
    }

    void Update()
    {
        // Se estiver dando Dash, não executa o movimento normal
        if (isDashing)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");

        // Movimento normal
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocity.y);

        // Guarda a direção em que o jogador está andando
        if (moveHorizontal > 0)
        {
            lastDirection = 1f;
        }
        else if (moveHorizontal < 0)
        {
            lastDirection = -1f;
        }

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
        }

        // DASH com Q
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        // Dá o impulso na última direção que o jogador estava andando
        rb.linearVelocity = new Vector2(lastDirection * dashSpeed, 0f);

        // Duração do Dash
        yield return new WaitForSeconds(dashDuration);

        // Para o Dash
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        isDashing = false;

        // Tempo para poder usar novamente
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
    void ReceberDano(float quantidade) // Recebe a quantidade de dano.
    {
        if (vidaAtual <= 0) // Verifica se o jogador já está sem vida.
            return; // Impede que continue recebendo dano.

        vidaAtual -= quantidade; // Diminui a vida do jogador.
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima); // Mantém a vida entre zero e o máximo.
        barraVida.value = vidaAtual; // Atualiza a barra de vida.

        if (vidaAtual <= 0) // Verifica se a vida chegou a zero.
        {
            menu.MostrarGameOver(); // Mostra a tela de Game Over.
        }
    } // Fecha o método ReceberDano.

    void OnCollisionEnter2D(Collision2D collision) // Detecta colisões físicas.
    {
        if (collision.gameObject.CompareTag("Ground")) // Verifica se tocou no chão.
        {
            isGrounded = true; // Informa que o jogador está no chão.
        }

        if (collision.gameObject.CompareTag("Dano")) // Verifica se tocou em um perigo.
        {
            ReceberDano(danoInimigo); // Retira vida ao tocar no inimigo.
        }

        if (collision.gameObject.CompareTag("Limbo")) // Verifica se caiu no Limbo.
        {
            menu.MostrarGameOver(); // Mata o jogador instantaneamente.
        }
    }

    void OnTriggerEnter2D(Collider2D other) // Detecta contatos com objetos Trigger.
    {
        if (other.gameObject.CompareTag("Dano")) // Verifica se entrou em contato com o inimigo.
        {
            ReceberDano(danoInimigo); // Retira vida do jogador.
        }

        if (other.gameObject.CompareTag("BlocoVitoria")) // Verifica se tocou no bloco de vitória.
        {
            menu.MostrarVitoria(); // Mostra a tela de vitória.
        }
    }

    void OnCollisionExit2D(Collision2D collision) // Detecta quando o jogador deixa uma colisão.
    {
        if (collision.gameObject.CompareTag("Ground")) // Verifica se saiu do chão.
        {
            isGrounded = false; // Informa que o jogador não está mais no chão.
        }
    }
}