using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    // Configurações do Dash
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isDashing = false;
    private bool canDash = true;

    // Guarda a última direção em que o jogador andou
    private float lastDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Dano"))
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}