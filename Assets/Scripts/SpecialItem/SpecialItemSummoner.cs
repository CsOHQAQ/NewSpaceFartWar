using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class SpecialItemSummoner : MonoBehaviour
{
    public float summonInterval=20f;
    private float curCount=0;
    private SpecialItem[] specialItems;
    void Start()
    {
        specialItems = ResourceManager.Instance.LoadAll<SpecialItem>("Prefabs/Item");
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
        SpecialItem item = ResourceManager.Instance.Instantiate($"Prefabs/Item/{specialItems[itemID].name}").GetComponent<SpecialItem>();
        float screenX= Camera.main.orthographicSize * Camera.main.aspect, screenY= Camera.main.orthographicSize;
        int direction= Random.Range(0, 4);
        float x=0, y=0;
        switch(direction)
        { 
            case 0://ио
                {
                    x = (Random.value * 2 - 1) * screenX; y = screenY + 5f;
                    item.transform.position = new Vector3(x, y);
                    break;
                }
                case 1://об
                {
                    x = (Random.value * 2 - 1) * screenX; y = -screenY - 5f;
                    item.transform.position = new Vector3(x, y);
                    break;
                }
                case 2://вС
                {
                    x = -screenX - 5f; y = (Random.value * 2 - 1) * screenY;
                    item.transform.position = new Vector3(x, y);
                    break;
                }
                case 3://ср
                {
                    x = screenX + 5f; y = (Random.value * 2 - 1) * screenY;
                    item.transform.position = new Vector3(x, y);
                    break;
                }
        }

        item.GetComponent<Rigidbody2D>().velocity = (new Vector3(x, y, 0) - new Vector3()).normalized * 1.5f;
        item.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-1f, 1f);
    }
}
