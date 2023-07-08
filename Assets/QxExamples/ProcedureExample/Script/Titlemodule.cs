using QxFramework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Titlemodule : Submodule
{
    private List<PlayerController> players = new List<PlayerController>();
    private bool owari;
    private Transform foucs;
    private float kxbCounter;

    protected override void OnInit()
    {
        base.OnInit();
        InitGame();
        MessageManager.Instance.Get<PlayerMessage>().RegisterHandler(PlayerMessage.Die, OnPlayerDie);
    }

    private void InitGame()
    {
        owari = false;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 15;
        QXData.Instance.SetTableAgent();
        GameMgr.Instance.InitModules();
        LevelManager.Instance.OpenLevel("Game", LoadCompleted);
    }

    private void LoadCompleted(string levelName)
    {
        OpenUI("HealthBarUI");
        var list = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in list)
        {
            players.Add(p.GetComponent<PlayerController>());
        }
    }

    private void OnPlayerDie(object sender, EventArgs args)
    {
        foreach (var p in players)
        {
            if (p != sender as PlayerController)
            {
                p.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                foucs = p.transform;
                owari = true;
                kxbCounter = 1f;
                break;
            }
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (owari && foucs != null)
        {
            if (kxbCounter > 0)
            {
                kxbCounter -= Time.deltaTime;
            }
            else
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(foucs.position.x, foucs.position.y, -10), 0.1f);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 1.5f, 0.1f);
                if (UIManager.Instance.GetUI("RetryUI") == null)
                {
                    OpenUI("RetryUI");
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LevelManager.Instance.CloseLevel();
        MessageManager.Instance.Get<PlayerMessage>().RemoveAbout(this);
    }
}
