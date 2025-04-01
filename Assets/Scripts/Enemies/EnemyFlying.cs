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
    [SerializeField] Transform shootPoint;

    private new void Start()
    {
        movePoint = transform.position;
        base.Start();
    }
    private void Update()
    {
        //Checking player
        var ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
        if (Physics.Raycast(ray, out var hit, 1000, attackLayers))
        {
            Debug.DrawLine(transform.position, Player.Instance.transform.position, Color.blue);
            if (hit.collider.CompareTag("Player"))
                Debug.Log(hit.collider.name + " / " + stopShooting + " / " + Vector3.Distance(Player.Instance.transform.position, transform.position).ToString());
            if (hit.collider.CompareTag("Player") && (!stopShooting) && Vector3.Distance(Player.Instance.transform.position, transform.position) < detectionDistance)
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

        //Shooting
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

        //Check movement and walls
        Debug.DrawLine(transform.position, movePoint, Color.red);
        var moveRay = new Ray(transform.position, movePoint - transform.position);
        if (Physics.Raycast(moveRay, maxMoveDistance, moveLayers) || Vector3.Distance(movePoint, transform.position) < 0.5f)
        { movePoint = transform.position + Random.insideUnitSphere * Random.Range(0f, 1f) * maxMoveDistance; }
        else 
        { transform.position += (movePoint - transform.position) * currentSpeed * Time.deltaTime; }
            HandleRotation();

    }

    private void HandleRotation()
    {
        if (isShooting)
        { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Player.Instance.transform.position - transform.position), Time.deltaTime * 2); }
        else
        { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movePoint - transform.position), Time.deltaTime * 2);  }
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, shootPoint.position,Quaternion.LookRotation(Player.Instance.transform.position - transform.position));
        bullet.SetDirection((Player.Instance.transform.position - transform.position) / 10, gameObject);
    }

}
