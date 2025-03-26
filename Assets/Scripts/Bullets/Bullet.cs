using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected GameObject parent;
    protected Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] float bulletLife = 4f;

    public void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        bulletLife -= Time.deltaTime;
        if (bulletLife <= 0)
            Destroy(this.gameObject);
    }

    public void SetDirection(Vector3 direction, GameObject parent)
    {
        this.direction = direction;
        this.parent = parent;
    }
}
