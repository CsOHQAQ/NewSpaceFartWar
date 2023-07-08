using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeJudge : MonoBehaviour
{
    private float screenX,
        screenY;
    private List<PlayerController> players= new List<PlayerController>();
    // Start is called before the first frame update
    void Start()
    {
        screenX = Camera.main.orthographicSize * Camera.main.aspect;
        screenY = Camera.main.orthographicSize;
        var list=  GameObject.FindGameObjectsWithTag("Player");
        foreach(var p in  list)
        {
            players.Add(p.GetComponent<PlayerController>());
            Debug.Log($"边缘检测器已添加玩家{players[players.Count - 1].playerIndex}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var p in players)
        {
            if ((p.transform.position.x<-screenX||p.transform.position.x>screenX)||
                (p.transform.position.y < -screenY || p.transform.position.y > screenY))
            {
                Debug.LogWarning($"玩家{p.playerIndex}已位于界外！");
            }
        }
    }
    
}
