using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int cantidadEnemigos;
    public int delayEntreEnemigos; // segundos (int)
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Configuración Oleadas")]
    public Wave[] oleadas;
    public Transform[] waypoints;
    public EnemyManager enemyManager;
    public Castle castle;
    public GameManager gameManager;

    [Header("Configuración Inicial")]
    public int tiempoPreparacionInicial = 10; // Tiempo inicial ANTES de empezar
    public int tiempoParaReiniciar = 10; // Tiempo inicial ANTES de empezar
    public TextMeshProUGUI delayUIText;
    public GameObject texto;
    private Queue<GameObject> colaEnemigos = new Queue<GameObject>();



    


    void Start()
    {
        StartCoroutine(ControladorOleadas());
    }

    IEnumerator ControladorOleadas()
    {

        
        // CONTADOR INICIAL claramente visible ANTES DE OLEADAS
        int tiempoRestante = tiempoPreparacionInicial;
        while (tiempoRestante > 0)
        {
            delayUIText.text =  tiempoRestante.ToString();
            yield return new WaitForSeconds(1f);
            tiempoRestante--;
        }

        delayUIText.text = "¡Comienza la batalla!";
        yield return new WaitForSeconds(1f);
        texto.SetActive(false);
        // AHORA SÍ empieza claramente la generación de oleadas
        foreach (Wave wave in oleadas)
        {
            // Llena la cola claramente con enemigos de la oleada actual
            for (int i = 0; i < wave.cantidadEnemigos; i++)
                colaEnemigos.Enqueue(wave.enemyPrefab);

            // Spawnea enemigos claramente con delay entre ellos
            while (colaEnemigos.Count > 0)
            {
                GameObject enemyToSpawn = colaEnemigos.Dequeue();
                SpawnEnemy(enemyToSpawn);

                // Delay claramente visible en UI
                for (int delay = wave.delayEntreEnemigos; delay > 0; delay--)
                {
                    delayUIText.text = delay.ToString();
                    yield return new WaitForSeconds(1f);
                }
            }
        }
        texto.SetActive(true);
        delayUIText.text = "¡Todas las oleadas terminadas!";
        yield return new WaitForSeconds(1f);
        delayUIText.text = "Reiniciando";
        int tiempoReinicio = tiempoParaReiniciar;
        while (tiempoReinicio > 0)
        {
            delayUIText.text = tiempoReinicio.ToString();
            yield return new WaitForSeconds(1f);
            tiempoReinicio--;
        }
        
        RestartScene();
        
    }

    void SpawnEnemy(GameObject prefab)
    {
        GameObject enemyGO = Instantiate(prefab, waypoints[0].position, Quaternion.identity);
        Enemy enemyScript = enemyGO.GetComponent<Enemy>();

        enemyScript.Initialize(waypoints, enemyManager, castle, gameManager);
        enemyManager.AddEnemy(enemyGO);
    }
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
