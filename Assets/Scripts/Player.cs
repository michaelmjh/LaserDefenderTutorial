using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerMoveSpeed;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed;
    [SerializeField] float laserFiringPeriod;

    Coroutine firingCoroutine;

    // Movement boundaries for player
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float padding = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        FireProjectile();
    }

    private void MovePlayer()
    {
        // Get player input and use to calculate movement direction and speed
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * playerMoveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * playerMoveSpeed;
        // Calculate position to move to within movement boundaries
        var newXPosition = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        // Move player
        transform.position = new Vector2(newXPosition, newYPosition);
    }

    private void FireProjectile()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject playerProjectile = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            playerProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed); ;
            yield return new WaitForSeconds(laserFiringPeriod);
        }
        
    }
}
