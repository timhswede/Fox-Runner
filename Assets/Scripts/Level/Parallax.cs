using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public float depth = 1;
    Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= (player.velocity.x / depth) * Time.fixedDeltaTime;
        if (pos.x <= -55) { pos.x = 80; }
        transform.position = pos;
    }
}
