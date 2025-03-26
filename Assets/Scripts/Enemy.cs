using UnityEngine;

public enum EnemyState { Caminando, Atacando, Muriendo, Muerto }

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public int maxHealth = 10;
    public int damage = 1;
    public int moneyReward = 5;

    public int currentHealth;
    public EnemyState state = EnemyState.Caminando;

    private int waypointIndex = 0;
    private Transform[] waypoints;
    private EnemyManager enemyManager;
    private Castle targetCastle;
    private GameManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(Transform[] pathPoints, EnemyManager manager, Castle castle, GameManager gm)
    {
        waypoints = pathPoints;
        enemyManager = manager;
        targetCastle = castle;
        gameManager = gm;
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Caminando:
                MoveAlongPath();
                break;

            case EnemyState.Atacando:
                AttackCastle();
                break;

            case EnemyState.Muriendo:
                HandleDeath();
                break;

            case EnemyState.Muerto:
                // no hacer nada
                break;
        }
    }

    void MoveAlongPath()
    {
        if (waypoints == null || waypointIndex >= waypoints.Length)
            return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
                state = EnemyState.Atacando;
        }
    }

    void AttackCastle()
    {
        targetCastle.TakeDamage(damage);
        enemyManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        if (state == EnemyState.Muerto || state == EnemyState.Muriendo)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
            state = EnemyState.Muriendo;
    }

    void HandleDeath()
    {
        enemyManager.RemoveEnemy(gameObject);
        gameManager.EarnMoney(moneyReward);

        var sprite = GetComponent<SpriteRenderer>();
        if (sprite) sprite.enabled = false;

        var collider = GetComponent<Collider2D>();
        if (collider) collider.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb) rb.simulated = false;

        state = EnemyState.Muerto;
        Destroy(gameObject, 0.05f);
    }

    public bool IsDead => state == EnemyState.Muriendo || state == EnemyState.Muerto;
}
