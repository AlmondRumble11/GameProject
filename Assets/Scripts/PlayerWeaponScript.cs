using UnityEngine;

public class PlayerWeaponScript : MonoBehaviour
{

    // Projectile that the player shoots
    public GameObject projectile;
    public GameObject areaProjectile;
    public GameObject upgradedProjectile;

    // Were the projectiles are fired
    public Transform firePoint;

    // Force to shoot the projectiles
    public float fireForce = 20;

    // Camera location
    public Camera mainCamera;

    // Offset
    public float weaponOffset;



    void Update()
    {
        // How to shoot: https://www.youtube.com/watch?v=mgjWA2mxLfI
        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePosition - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotate the weapon to face the mouse position
        transform.rotation = Quaternion.Euler(0f, 0f, angle + weaponOffset);
    }

    // Shoot different projectile prefab based on the current projectile
    public void PlayerWeaponFire(ProjectileType type)
    {

        if (type == ProjectileType.Upgraded)
        {
            Instantiate(upgradedProjectile, firePoint.position, transform.rotation);
        }
        else if (type == ProjectileType.AreaShot)
        {
            Instantiate(areaProjectile, firePoint.position, transform.rotation);
        }
        else
        {
            Instantiate(projectile, firePoint.position, transform.rotation);
        }
    }
}
