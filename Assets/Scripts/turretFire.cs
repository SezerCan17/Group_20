using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretFire : MonoBehaviour
{
    public Transform currentEnemy;
    public List<Enemy> enemies = new List<Enemy>();
    public int hitPoint;
    public Transform[] firePoints;
    float fireRate = .5f;
    float nextFireTime = 0.0f;
    public GameObject fireParticle;
    public GameObject hitParticle;
    public AudioSource turretAus;
    public AudioClip shootSFX;
    public Transform turretHead;
    int turretLevel = 1;
    public TextMeshProUGUI turretLevelText;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.GetComponent<Enemy>());
            Debug.Log("Düşman algılandı!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.GetComponent<Enemy>());
            Debug.Log("Düşman algılama alanından çıktı!");
        }
    }
    public void UpGradeTurret()
    {
        turretLevel++;
        fireRate += turretLevel / 10;
        hitPoint = turretLevel;

        turretLevelText.text = "Level " + turretLevel.ToString() + " Turret";
    }

    void Update()
    {

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        if (enemies.Count > 0 )
        {
            currentEnemy = enemies.Last().transform;

            Enemy enemyComponent = currentEnemy.gameObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                if (currentEnemy.gameObject.GetComponent<Enemy>().isDead == false)
                {
                    Vector3 direction = currentEnemy.position - transform.position;
                    direction.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    turretHead.rotation = Quaternion.Slerp(turretHead.rotation, rotation, Time.deltaTime * 5.0f);
                    if (Time.time >= nextFireTime)
                    {
                        Fire();
                        nextFireTime = Time.time + 1.0f / fireRate;
                    }
                }
                else
                {
                    enemies.Remove(enemyComponent);
                    currentEnemy = null;
                }
            }
            else
            {
                enemies.Remove(enemyComponent);
                currentEnemy = null;
            }
                


            
        }
    }

    void Fire()
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject fireEffect = Instantiate(fireParticle, firePoint.position, firePoint.rotation);
            Destroy(fireEffect, 1f);
        }

        turretAus.PlayOneShot(shootSFX);
        currentEnemy.GetComponent<Enemy>().GetHit(hitPoint);
    }
}
