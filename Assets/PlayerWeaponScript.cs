using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

    // Projectile that the player shoots
    public GameObject projectile;

    // Were the projectiles are fired
    public Transform firePoint;

    // Force to shoot the projectiles
    public float fireForce = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayerWeaponFire()
    {
        var newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}
