using Unity.VisualScripting;
using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionDamage;

    [SerializeField] GameObject missileMesh;
    [SerializeField] MeshRenderer explosionMesh;
    [SerializeField] float explosionMeshTimer;
    private bool hasExploded = false;

    private void Awake()
    {
        explosionMesh.enabled = false;
    }
    private new void Update()
    {
        if (hasExploded)
        {
            explosionMeshTimer -= Time.deltaTime;
            if (explosionMeshTimer <= 0)
            { Destroy(this.gameObject); }
        }
        else {base.Update(); }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != parent)
        {
            Explode();
            hasExploded = true;
            explosionMesh.enabled = true;
            missileMesh.SetActive(false);
        }
    }
    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (Vector3.Distance(hit.transform.position, transform.position) > explosionRadius)
                { damageable.TakeDamage(Mathf.Round(explosionDamage / 10)); }
                else if (Vector3.Distance(hit.transform.position, transform.position) > explosionRadius / 2)
                { damageable.TakeDamage(Mathf.Round(explosionDamage / 2)); }
                else
                { damageable.TakeDamage(explosionDamage); }
            }
        }
    }
    

    public void OnValidate()
    {
         explosionMesh.transform.localScale = Vector3.one * 2 * explosionRadius;
    }
}
