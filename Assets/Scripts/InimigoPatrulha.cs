using UnityEngine;

public class InimigoPatrulha : MonoBehaviour // Cria a classe responsável pelo comportamento do inimigo.
{
    public Transform pontoA; // Guarda a posição inicial da patrulha.
    public Transform pontoB; // Guarda a posição final da patrulha.
    public Transform jogador; // Guarda a referência ao Player.

    public float velocidade = 2f; // Define a velocidade normal da patrulha.
    public float velocidadePerseguicao = 3.5f; // Define a velocidade durante a perseguição.
    public float distanciaPerseguicao = 5f; // Define a distância necessária para detectar o Player.

    private Rigidbody2D rb; // Guarda o componente físico do inimigo.
    private Transform destino; // Guarda o ponto atual da patrulha.
    private bool perseguindo = false; // Informa se o inimigo está perseguindo o jogador.

    void Start() // Executa uma vez quando o jogo começa.
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D do inimigo.
        destino = pontoB; // Define o Ponto B como primeiro destino.
    }

    void FixedUpdate() // Executa em intervalos fixos para controlar a movimentação.
    {
        if (rb == null || pontoA == null || pontoB == null || jogador == null) // Verifica se todas as referências necessárias existem.
            return; // Interrompe a execução caso falte alguma referência.

        float distancia = Vector2.Distance(transform.position, jogador.position); // Calcula a distância entre o inimigo e o jogador.

        if (distancia <= distanciaPerseguicao) // Verifica se o jogador está dentro do alcance de detecção.
        {
            perseguindo = true; // Ativa o modo de perseguição.
        }
        else // Executa quando o jogador está fora do alcance.
        {
            perseguindo = false; // Desativa o modo de perseguição.
        }

        if (perseguindo) // Verifica se o inimigo deve perseguir o jogador.
        {
            Vector2 posicaoJogador = new Vector2(jogador.position.x, rb.position.y); // Usa somente a posição horizontal do jogador.

            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, posicaoJogador, velocidadePerseguicao * Time.fixedDeltaTime); // Calcula o movimento em direção ao jogador.

            rb.MovePosition(novaPosicao); // Move o inimigo na direção do jogador.
        }
        else // Executa quando o inimigo não está perseguindo.
        {
            Vector2 posicaoDestino = new Vector2(destino.position.x, rb.position.y); // Usa somente a posição horizontal do ponto de patrulha.

            Vector2 novaPosicao = Vector2.MoveTowards(rb.position, posicaoDestino, velocidade * Time.fixedDeltaTime); // Calcula o movimento de patrulha.

            rb.MovePosition(novaPosicao); // Move o inimigo em direção ao ponto escolhido.

            if (Mathf.Abs(rb.position.x - destino.position.x) < 0.7f) // Verifica se o inimigo chegou perto do destino no eixo X.
            {
                destino = destino == pontoA ? pontoB : pontoA; // Alterna entre o Ponto A e o Ponto B.
            }
        }
    }
}



