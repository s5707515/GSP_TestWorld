using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletVelocity, bulletSpread, fireRate;
    private float lifeTime = 3;
    private bool isShooting, readyToShoot, allowReset = true;
    private int currentBurstBullet; 
    public int bulletsPerBurst = 3;
    
    public enum ShootingMode
    {
        Auto,
        Single,
        Burst
    }

    public ShootingMode currentMode;

    private void Awake()
    {
        readyToShoot = true;
        currentBurstBullet = bulletsPerBurst;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMode == ShootingMode.Auto)
        {
            isShooting = Input.GetMouseButton(0);
        }
        else if (currentMode == ShootingMode.Burst || currentMode == ShootingMode.Single )
        {
            isShooting = Input.GetMouseButtonDown(0);
        }

        if (readyToShoot && isShooting)
        {
            currentBurstBullet = bulletsPerBurst;
            Fire();
        }
    }

    private void Fire()
    {

        readyToShoot = false;

        Vector3 shotDirection = CalculateDirectionAndSpread().normalized;

        // Fix Bullet Orientation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.transform.forward = shotDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shotDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bullet, lifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", fireRate);
            allowReset = false;
        }

        if (currentMode == ShootingMode.Burst && currentBurstBullet > 1)
        {
            currentBurstBullet--;
            Invoke("Fire", fireRate);
        }
    }

    Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;


        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - firePoint.position;

        float x = UnityEngine.Random.Range(-bulletSpread, bulletSpread);
        float y = UnityEngine.Random.Range(-bulletSpread, bulletSpread);

        return direction + new Vector3(x, y, 0);

    }

    private IEnumerator DestroyBullet(GameObject bullet, float lifeTime)
    {

        yield return new WaitForSeconds (lifeTime);
            
        Destroy(bullet);

    }
}