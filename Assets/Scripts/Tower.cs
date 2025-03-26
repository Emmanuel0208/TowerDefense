using UnityEngine;
using System.Collections;

public enum TowerState { Idle, Atacando }

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 2;

    [Header("Raycast Personalizable")]
    public LineRenderer lineRenderer; // Asignar desde Inspector

    private float fireCountdown = 0f;
    private Enemy targetEnemy;
    private TowerState state = TowerState.Idle;

    void Update()
    {
        switch (state)
        {
            case TowerState.Idle:
                BuscarEnemigo();
                break;

            case TowerState.Atacando:
                AtacarEnemigo();
                break;
        }
    }

    void BuscarEnemigo()
    {
        Enemy[] enemigos = FindObjectsOfType<Enemy>();
        float distanciaCercana = Mathf.Infinity;
        Enemy enemigoCercano = null;

        foreach (var enemigo in enemigos)
        {
            if (enemigo.IsDead) continue;

            float distancia = Vector3.Distance(transform.position, enemigo.transform.position);
            if (distancia <= range && distancia < distanciaCercana)
            {
                distanciaCercana = distancia;
                enemigoCercano = enemigo;
            }
        }

        if (enemigoCercano)
        {
            targetEnemy = enemigoCercano;
            state = TowerState.Atacando;
        }
    }

    void AtacarEnemigo()
    {
        if (!targetEnemy || targetEnemy.IsDead || Vector3.Distance(transform.position, targetEnemy.transform.position) > range)
        {
            lineRenderer.enabled = false;
            targetEnemy = null;
            state = TowerState.Idle;
            return;
        }

        // Actualiza visualmente el rayo
        if (lineRenderer && lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetEnemy.transform.position);
        }


        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        StartCoroutine(ShootEffectCoroutine());
    }

    IEnumerator ShootEffectCoroutine()
    {
        if (lineRenderer && targetEnemy)
        {
            // Activa el rayo visualmente SOLO al disparar
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetEnemy.transform.position);
        }

        // Aplica daño al enemigo inmediatamente después
        if (targetEnemy && !targetEnemy.IsDead)
        {
            targetEnemy.TakeDamage(damage);
        }

        // Espera un breve instante para efecto visual del rayo
        yield return new WaitForSeconds(0.07f); // ajusta este tiempo para mayor o menor duración del rayo

        // Apaga el rayo hasta el próximo disparo
        if (lineRenderer)
            lineRenderer.enabled = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
