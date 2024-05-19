using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int hitPoints = 3;
    public bool isDead = false;
    public GameObject particleEffect;
    public GameObject deadPrefab;
    public Image healthImage;
    

    public void GetHit(int hitPoint)
    {
        if (hitPoints > 0)
        {
            hitPoints -= hitPoint;
            GameObject hitEffect = Instantiate(particleEffect, this.transform);

            hitEffect.transform.localPosition = new Vector3(0, .5f, 0); // Parent'ýn konumuna göre ayarla
            hitEffect.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            Destroy(hitEffect, 1f);
            ChangeUI();
            if (hitPoints <= 0)
            {
                Die();
            }
        }
        
    }

    void ChangeUI()
    {
        if(hitPoints == 2)
        {
            healthImage.rectTransform.sizeDelta = new Vector2(.66f, 0.2f);
        }
        else if (hitPoints == 1)
        {
            healthImage.rectTransform.sizeDelta = new Vector2(.33f, 0.2f);
        }
    }

    void Die()
    {
        isDead = true;
        Instantiate(deadPrefab, new Vector3(transform.position.x,.5f,transform.position.z),transform.rotation);
        Destroy(gameObject);
    }
}
