using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;

    GameObject results;
    public Text finalDistanceText;
    public Text highScoreDistanceText;
    public int highScoreDistance;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        highScoreDistanceText = GameObject.Find("HighScoreDistanceText").GetComponent<Text>();

        results.SetActive(false);
    }

    void Start()
    {
        highScoreDistanceText.text = PlayerPrefs.GetInt("highScoreDistance") + " m";
        highScoreDistance = PlayerPrefs.GetInt("highScoreDistance");

    }

    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";
        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + " m";
            highScoreDistance = Math.Max(highScoreDistance, distance);
            PlayerPrefs.SetInt("highScoreDistance", highScoreDistance);

        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
