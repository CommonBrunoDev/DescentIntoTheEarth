using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCrawler : Enemy
{
    private AICrawler AIScript;
    [SerializeField] LayerMask layersToHit;
    private bool isShooting = false;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float shootDelay;
    private float shootTimer;
    private Bullet bullet;

    [SerializeField] bool stopShooting = false;
    [SerializeField] float agentSpeed = 2;

    private void Start()
    {
        AIScript = GetComponent<AICrawler>();
    }

    private void Update()
    {
        var ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, 1000, layersToHit))
        {
            Debug.DrawLine(transform.position, hit.transform.position, new UnityEngine.Color(1f, 1f, 1.0f),1);
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Player") && (!stopShooting))
            {
                AIScript.LinkedAgent.speed = 0;
                isShooting = true;
            }
            else
            {
                AIScript.LinkedAgent.speed = agentSpeed;
                isShooting = false;
            }
        }
        
        if (isShooting)
        {
            if (shootTimer <= 0 && bullet == null)
            {
                Shoot();
                shootTimer = shootDelay;
            }
            else
            { shootTimer -= Time.deltaTime; }
        }

        if (AIScript.LinkedAgent.remainingDistance < 1)
        { Escape(); }
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.SetDirection((Player.Instance.transform.position - transform.position) / 10, gameObject);
    }

    private void Escape()
    {

    }
}
