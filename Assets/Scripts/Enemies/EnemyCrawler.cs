using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCrawler : Enemy
{
    private AICrawler AIScript;
    [SerializeField] LayerMask layersToHit;
    private bool isShooting = false;
    [SerializeField] EnemyBulletNormal bulletPrefab;
    [SerializeField] float shootDelay;
    private float shootTimer;

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
            if (hit.collider.CompareTag("Player"))
            {
                AIScript.LinkedAgent.speed = 0;
                isShooting = true;
            }
            else
            {
                AIScript.LinkedAgent.speed = 2;
                isShooting = false;
            }
        }
        
        if (isShooting)
        {
            if (shootTimer <= 0)
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
        var b = Instantiate(bulletPrefab, transform.position, transform.rotation);
        b.SetDirection(Player.Instance.transform.position - transform.position, gameObject);
    }

    private void Escape()
    {

    }
}
