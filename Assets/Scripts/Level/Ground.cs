using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight, groundRight, screenRight;
    bool didGenerateGround = false;
    public Obstacle boxTemplate;
    public ObstacleEnemy eagleTemplate;
    public Environment houseTemplate;
    public Pine pineTemplate;
    public Rock rockTemplate;
    public House2 house2Template;
    public Mushroom mushroomTemplate;
    public Bush bushTemplate;
    BoxCollider2D collider;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;
    }

    void Update()
    {
        groundHeight = transform.position.y + (collider.size.y / 2);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < -100)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }
        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float t = player.jumpVelocity / (-player.gravity);
        float maxJumpHeight = (player.jumpVelocity * player.maxHoldJumpTime) + (player.jumpVelocity * t + (0.5f * (player.gravity * (t * t))));
        float maxY = maxJumpHeight * 0.7f; maxY += groundHeight;
        float playerY = Random.Range(1, maxY);
        pos.y = playerY - goCollider.size.y / 2;
        if (pos.y > 2.7f) { pos.y = 2.7f; }
   

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - playerY)) / -player.gravity);
        float maxX = (t1 + t2) * player.velocity.x; maxX *= 0.7f; maxX += groundRight;
        float playerX = Random.Range(screenRight + 5, maxX);
        pos.x = playerX + goCollider.size.x / 2; 
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        int numPine = Random.Range(1, 3);
        for (int i = 0; i < numPine; i++)
        {
            GameObject pine = Instantiate(pineTemplate.gameObject);
            float y = goGround.groundHeight + 4;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 1);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 1);
            float x = Random.Range(left, right);
            Vector2 pinePos = new Vector2(x, y);
            pine.transform.position = pinePos;
        }

        int numRock = Random.Range(1, 3);
        for (int i = 0; i < numRock; i++)
        {
            GameObject rock = Instantiate(rockTemplate.gameObject);
            float y = goGround.groundHeight - 2;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 1);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 1);
            float x = Random.Range(left, right);
            Vector2 rockPos = new Vector2(x, y);
            rock.transform.position = rockPos;
        }

        int numMushroom = Random.Range(0, 3);
        for (int i = 0; i < numMushroom; i++)
        {
            GameObject mushroom = Instantiate(mushroomTemplate.gameObject);
            float y = goGround.groundHeight - 2;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 1);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 1);
            float x = Random.Range(left, right);
            Vector2 mushroomPos = new Vector2(x, y);
            mushroom.transform.position = mushroomPos;
        }

        int numBush = Random.Range(0, 3);
        for (int i = 0; i < numBush; i++)
        {
            GameObject bush = Instantiate(bushTemplate.gameObject);
            float y = goGround.groundHeight - 3;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 1);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 1);
            float x = Random.Range(left, right);
            Vector2 bushPos = new Vector2(x, y);
            bush.transform.position = bushPos;
        }

        int numHouse = Random.Range(0, 2);
        for (int i = 0; i < numHouse; i++)
        {
            GameObject house = Instantiate(houseTemplate.gameObject);
            float y = goGround.groundHeight + 3;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 5);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 5);
            float x = Random.Range(left, right);
            Vector2 housePos = new Vector2(x, y);
            house.transform.position = housePos;
        }

        int numHouse2 = Random.Range(0, 2);
        for (int i = 0; i < numHouse2; i++)
        {
            GameObject house2 = Instantiate(house2Template.gameObject);
            float y = goGround.groundHeight + 2;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 5);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 5);
            float x = Random.Range(left, right);
            Vector2 house2Pos = new Vector2(x, y);
            house2.transform.position = house2Pos;
        }

        int numObstacle = Random.Range(0, 4);
        for (int i=0; i < numObstacle; i++)
        {
            GameObject box = Instantiate(boxTemplate.gameObject);
            float y = goGround.groundHeight - 1;
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 1);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 1);
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;

        }

        int numEagle = Random.Range(0, 2);
        for (int i = 0; i < numEagle; i++)
        {
            GameObject eagleEnemy = Instantiate(eagleTemplate.gameObject);
            float randomNumber = Random.Range(0, 2);
            float y = goGround.groundHeight;
            if (randomNumber == 0)
            {
                y = goGround.groundHeight + 5;
            }
            else if (randomNumber == 1)
            {
                y = goGround.groundHeight + 3;
            }
            else
            {
                y = goGround.groundHeight + 1;
            }
            float left = go.transform.position.x - ((goCollider.size.x / 2) - 2);
            float right = go.transform.position.x + ((goCollider.size.x / 2) - 2);
            float x = Random.Range(left, right);
            Vector2 eaglePos = new Vector2(x, y);
            eagleEnemy.transform.position = eaglePos;

        }
    }

}
