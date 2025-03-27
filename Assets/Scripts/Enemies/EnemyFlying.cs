using UnityEngine;

public class EnemyFlying : Enemy
{
    [SerializeField] float maxMoveDistance = 10;
    public Vector3 movePoint;

    [SerializeField] bool stopShooting = false;
    [SerializeField] float speed = 2;
    [SerializeField] float currentSpeed = 2;

    [SerializeField] LayerMask moveLayers;
    [SerializeField] LayerMask attackLayers;
    private bool isShooting = false;

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float shootDelay;
    private float shootTimer;
    private Bullet bullet;

    [SerializeField] Transform meshTransform;

    private void Start()
    {
        movePoint = transform.position;
    }
    private void Update()
    {
        var ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, 1000, attackLayers))
        {
            Debug.DrawLine(transform.position, hit.transform.position, new UnityEngine.Color(1f, 1f, 1.0f), 1);
            if (hit.collider.CompareTag("Player") && (!stopShooting))
            {
                isShooting = true;
                currentSpeed = speed / 2;
            }
            else
            {
                isShooting = false;
                currentSpeed = speed;
            }
        }

        if(isShooting)
        {
            if (shootTimer <= 0 && bullet == null)
            {
                Shoot();
                shootTimer = shootDelay;
            }
            else
            { shootTimer -= Time.deltaTime; }
        }

        var moveRay = new Ray(transform.position, movePoint);
        if (Physics.Raycast(moveRay, out var moveHit, 0.1f, moveLayers))
        {
            Debug.Log(moveHit.collider.CompareTag("Wall"));
            Debug.Log(Vector3.Distance(movePoint, transform.position));
            if (moveHit.collider.CompareTag("Wall") || Vector3.Distance(movePoint, transform.position) < 0.1)
            { 
                movePoint = transform.position + Random.insideUnitSphere * maxMoveDistance;
                Debug.Log("Relocated");
            }
        }

        Debug.Log("Drawed line");
        Debug.DrawLine(transform.position, movePoint, new UnityEngine.Color(1f, 1f, 1.0f), 1);

        HandleRotation();

        transform.position += (movePoint - transform.position) * currentSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        if (isShooting)
        { meshTransform.rotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position); }
        else
        { meshTransform.rotation = Quaternion.LookRotation(movePoint - transform.position); }
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.SetDirection((Player.Instance.transform.position - transform.position) / 10, gameObject);
    }

}
