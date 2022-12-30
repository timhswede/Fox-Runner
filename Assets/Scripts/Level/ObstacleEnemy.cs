using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleEnemy : MonoBehaviour
{
    Player player;
    [SerializeField] private float damage;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(1);

        }
    }



    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime * 2;
       
        if (pos.x < -100)
        {
            Destroy(gameObject);
        }
        transform.position = pos;
    }
}
