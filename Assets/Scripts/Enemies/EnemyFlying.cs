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

        //Checking player
        Debug.Log(Vector3.Distance(movePoint, transform.position));
        if (Vector3.Distance(movePoint, transform.position) < 20)
        {
            var ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
            if (Physics.Raycast(ray, out var hit, 1000, attackLayers))
            {
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
        }

        //Shooting
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

        //Check movement and walls
        var moveRay = new Ray(transform.position, movePoint - transform.position);
        if (Physics.Raycast(moveRay, maxMoveDistance, moveLayers) || Vector3.Distance(movePoint, transform.position) < 1.5f)
        { movePoint = transform.position + Random.insideUnitSphere * Random.Range(0f, 1f) * maxMoveDistance; }
        else 
        { transform.position += (movePoint - transform.position) * currentSpeed * Time.deltaTime; }
            HandleRotation();

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
