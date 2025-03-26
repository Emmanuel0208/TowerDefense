using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Importante para reiniciar escena

public class Castle : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("¡El castillo ha sido destruido!");

            // 🔥 AQUÍ REINICIAMOS claramente LA ESCENA:
            RestartScene();
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
