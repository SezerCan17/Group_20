using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera mainCamera;
    private Rigidbody rb;
    public float rotationThreshold = 1f;
    public Animator playerAnimator;
    public float stepInterval = 1f;
    private float lastStepTime;
    public AudioSource audioSource;



    [Serializable]
    public struct MaterialFootstepPair
    {
        public PhysicMaterial material;
        public AudioClip[] footstepSounds;
    }

    public AudioClip shotClip;


    public MaterialFootstepPair[] materialFootstepPairs;
    public LayerMask raycastLayerMask;
    public GameObject groundCheck;

    public GameObject laser;

    float shootTimer = 0f;

    public Transform projectileSpawn;
    public GameObject projectile;
    public float projectileSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        Move();
        RotateTowardsMouse();
        Fire();

        shootTimer -= Time.deltaTime;
    }


    void Fire()
    {
        if(Input.GetMouseButton(0) && shootTimer < 0f)
        {
            if(playerAnimator.GetFloat("Speed") < 0.1f)
            {
                playerAnimator.SetTrigger("Fire");
            }
            shootTimer = 1.3f;
            StartCoroutine(FireProjectile());
        }
    }

    IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject instantiatedProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        instantiatedProjectile.GetComponent<Rigidbody>().AddForce(projectileSpawn.forward * projectileSpeed, ForceMode.Impulse);
        Destroy(instantiatedProjectile, 3f);
        audioSource.PlayOneShot(shotClip);
        
        Debug.Log("Ateþ Ettim");
    }
    void Move()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += transform.right;
        }

        rb.velocity = movement.normalized * moveSpeed;

        playerAnimator.SetFloat("Speed", movement.magnitude);

        if (movement != Vector3.zero)
        {

            if (!laser.gameObject.activeSelf)
            {
                StartCoroutine(ShowLaser());
            }

            RaycastHit hit;
            if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, Mathf.Infinity, raycastLayerMask))
            {
                PhysicMaterial hitMaterial = hit.collider.sharedMaterial;
                if (hitMaterial != null)
                {
                    foreach (var pair in materialFootstepPairs)
                    {
                        if (pair.material == hitMaterial)
                        {
                            if (Time.time - lastStepTime > stepInterval)
                            {
                                audioSource.PlayOneShot(pair.footstepSounds[UnityEngine.Random.Range(0, pair.footstepSounds.Length)]);
                                lastStepTime = Time.time;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (laser.gameObject.activeSelf)
            {
                StartCoroutine(HideLaser());
            }
        }

    }


    IEnumerator HideLaser()
    {
        yield return new WaitForSeconds(0f);
        laser.SetActive(false);
    }

    IEnumerator ShowLaser()
    {
        yield return new WaitForSeconds(0.2f);
        laser.SetActive(true);
    }
    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = (pointToLook - transform.position).normalized;
            direction.y = 0;


            if (Vector3.Distance(transform.position, pointToLook) > rotationThreshold)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                rb.rotation = lookRotation;
            }
        }
    }
}
