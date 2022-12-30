using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite jumpSprite;
    public Sprite fallSprite;

    public float gravity;
    public Vector2 velocity;
    public float maxXVelocity = 100, jumpVelocity = 20;
    public float maxAcceleration = 10, acceleration = 10;
    public float distance = 0, groundHeight = 12;
    public bool isGrounded = false, isHoldingJump = false, isDead = false;
    public float maxHoldJumpTime = 0.3f, maxMaxHoldJumpTime = 0.3f, holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 1;

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;

    public AudioSource audioSource;

    GroundFall fall;
    CameraController cameraController;

    void ChangeSpriteJump()
    {
        spriteRenderer.sprite = jumpSprite;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
                ChangeSpriteJump();

                if (fall != null)
                {
                    fall.player = null;
                    fall = null;
                    cameraController.StopShaking();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        Vector2 pos = transform.position;

        if (pos.y < -20)
        {
            isDead = true;
            
        }

        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }

                    fall = ground.GetComponent<GroundFall>();
                    if (fall != null)
                    {
                        fall.player = this;
                        cameraController.StartShaking();
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }
        }

        distance += velocity.x * Time.fixedDeltaTime;
        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            if (fall != null)
            {
                rayDistance = -fall.fallSpeed * Time.fixedDeltaTime;
            }
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        }

        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }

            ObstacleEnemy eagleEnemy = obstHitX.collider.GetComponent<ObstacleEnemy>();
            if (eagleEnemy != null)
            {
                hitObstacleEnemy(eagleEnemy);
            }
        }

        RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }

            ObstacleEnemy eagleEnemy = obstHitY.collider.GetComponent<ObstacleEnemy>();
            if (eagleEnemy != null)
            {
                hitObstacleEnemy(eagleEnemy);
            }
        }
        transform.position = pos;
    }


    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        velocity.x *= 0.7f;
        audioSource.Play();

    }

    public Health health;
    void hitObstacleEnemy(ObstacleEnemy eagleEnemy)
    {
        Destroy(eagleEnemy.gameObject);
        velocity.x *= 0.4f;
        health.currentHealth -= 1;
        audioSource.Play();
        if (health.currentHealth == 0) {
            isDead = true;
        }

    }



}
