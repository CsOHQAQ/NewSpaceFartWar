using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class SpecialItemSummoner : MonoBehaviour
{   
    private float summonInterval=6f;
    private float curCount=0f;
    private GameObject[] specialItems;
    void Start()
    {
        specialItems = ResourceManager.Instance.LoadAll<GameObject>("Prefabs/Item");
    }

    // Update is called once per frame
    void Update()
    {
        curCount += Time.deltaTime;
        if(curCount > summonInterval)
        {
            curCount = 0;
            SummonItem();
        }
    }

    public void SummonItem()
    {
        int itemID=Random.Range(0, specialItems.Length);
        Debug.Log($"准备生成{specialItems[itemID].name}");
        //GameObject testObj= ResourceManager.Instance.Instantiate("Prefabs/Item/BeanPot");
        GameObject go = ResourceManager.Instance.Instantiate($"Prefabs/Item/{specialItems[itemID].name}");
        float screenX= Camera.main.orthographicSize * Camera.main.aspect, screenY= Camera.main.orthographicSize;
        int direction= Random.Range(0, 4);
        float x=0, y=0;
        switch(direction)
        { 
            case 0://上
                {
                    x = (Random.value * 2 - 1) * screenX; y = screenY + 5f;
                    go.transform.position = new Vector3(x, y);
                    break;
                }
                case 1://下
                {
                    x = (Random.value * 2 - 1) * screenX; y = -screenY - 5f;
                    go.transform.position = new Vector3(x, y);
                    break;
                }
                case 2://左
                {
                    x = -screenX - 5f; y = (Random.value * 2 - 1) * screenY;
                    go.transform.position = new Vector3(x, y);
                    break;
                }
                case 3://右
                {
                    x = screenX + 5f; y = (Random.value * 2 - 1) * screenY;
                    go.transform.position = new Vector3(x, y);
                    break;
                }
        }

        go.GetComponent<Rigidbody2D>().velocity = new Vector3(-x,-y).normalized * 3f;
        go.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1f, 1f);
    }
}
