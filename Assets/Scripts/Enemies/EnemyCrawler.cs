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

    [SerializeField] Transform meshTransform;
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform exitPoint;

    private new void Start()
    {
        AIScript = GetComponent<AICrawler>();
        AIScript.LinkedAgent.SetDestination(exitPoint.position);
        base.Start();
    }

    private void Update()
    {
        var ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, 1000, layersToHit))
        {
            if (hit.collider.CompareTag("Player") && (!stopShooting) && Vector3.Distance(Player.Instance.transform.position, transform.position) < detectionDistance)
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Player.Instance.transform.position - transform.position), Time.deltaTime * 2);
            if (shootTimer <= 0 && bullet == null)
            {
                Shoot();
                shootTimer = shootDelay;
            }
            else
            { shootTimer -= Time.deltaTime; }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(AIScript.LinkedAgent.desiredVelocity), Time.deltaTime * 2);
        }
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(Player.Instance.transform.position - transform.position));
        bullet.SetDirection((Player.Instance.transform.position - transform.position) / 10, gameObject);
    }
}
