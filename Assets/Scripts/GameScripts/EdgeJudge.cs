using QxFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeJudge : MonoBehaviour
{
    private float screenX, screenY;
    private List<PlayerController> players= new List<PlayerController>();
    // Start is called before the first frame update
    void Start()
    {
        screenX = Camera.main.orthographicSize * Camera.main.aspect;
        screenY = Camera.main.orthographicSize;
        var list=  GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in list)
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
            if (p.isDead)
            {
                continue;
            }
            if (p.transform.position.x < -screenX)
            {
                GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/OutEffect");
                go.transform.position = p.transform.position;
                go.transform.eulerAngles = new Vector3(0, 0, 0);
                MessageManager.Instance.Get<OffsetControlType>().DispatchMessage(OffsetControlType.Shake, p, new OffsetArgs(0.5f, 0.3f));
                p.Die();
                Debug.LogWarning($"玩家{p.playerIndex}已位于界外！");
            }
            if (p.transform.position.x > screenX)
            {
                GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/OutEffect");
                go.transform.position = p.transform.position;
                go.transform.eulerAngles = new Vector3(0, 0, 180);
                MessageManager.Instance.Get<OffsetControlType>().DispatchMessage(OffsetControlType.Shake, p, new OffsetArgs(0.5f, 0.3f));
                p.Die();
                Debug.LogWarning($"玩家{p.playerIndex}已位于界外！");
            }
            if (p.transform.position.y < -screenY)
            {
                GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/LinOutEffecte2");
                go.transform.position = p.transform.position;
                go.transform.eulerAngles = new Vector3(0, 0, 90);
                MessageManager.Instance.Get<OffsetControlType>().DispatchMessage(OffsetControlType.Shake, p, new OffsetArgs(0.5f, 0.3f));
                p.Die();
                Debug.LogWarning($"玩家{p.playerIndex}已位于界外！");
            }
            if (p.transform.position.y > screenY)
            {
                GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Effect/OutEffect");
                go.transform.position = p.transform.position;
                go.transform.eulerAngles = new Vector3(0, 0, 270);
                MessageManager.Instance.Get<OffsetControlType>().DispatchMessage(OffsetControlType.Shake, p, new OffsetArgs(0.5f, 0.3f));
                p.Die();
                Debug.LogWarning($"玩家{p.playerIndex}已位于界外！");
            }
        }
    }
    
}
