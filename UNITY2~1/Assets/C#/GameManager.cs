﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestarBtn;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        //Change Stage
        if (stageIndex < Stages.Length - 1 )
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else {// Game Clear
            //Player Contol Lock
            Time.timeScale = 0;

            //Result UI
            Debug.Log("게임 클리어!");

            //Restart Button UI
            Text btnText = UIRestarBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            UIRestarBtn.SetActive(true);
        }

        //Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 1) {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
            
        else {
            //All Health UI off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);

            // Player Die Effect
            player.OnDie();

            //Retry Button
            UIRestarBtn.SetActive(true);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            //Player Reposition
            if (health > 1) 
                PlayerReposition();
            

            //Health Down
            HealthDown();
        }
        
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-5.13f, 2.54f, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
