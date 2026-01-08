using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float fireCooldown = 0.15f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private PlayerMotor motor;
    private PlayerShooter shooter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        motor = new PlayerMotor(speed);
        shooter = new PlayerShooter(fireCooldown);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shooter.CanFire(Time.time))
        {
            Shoot();
            shooter.MarkFired(Time.time);
        }
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rb.linearVelocity = motor.ComputeVelocity(input);
    }

    void Shoot()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        Vector2 dir = (mouse - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Launch(dir);
    }
}
