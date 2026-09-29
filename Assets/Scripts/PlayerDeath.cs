using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject telaMorte;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dano"))
        {
            telaMorte.SetActive(true);
        }












    }
}
