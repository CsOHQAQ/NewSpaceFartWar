using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using QxFramework.Core;
public class DragOnPic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //数据信息
    [HideInInspector]
    public CargoData cargo;
    [HideInInspector]
    public CargoData othercargo;
    [HideInInspector]
    public ItemPile itemPile;
    [HideInInspector]
    public int PosID;
    [HideInInspector]
    public bool CanDrag;
    [HideInInspector]
    public bool Draging;

    //记录下自己的父物体.
    Transform myParent;

    //Panel，使拖拽是显示在最上方.
    Transform tempParent;

    CanvasGroup cg;
    RectTransform rt;

    //记录鼠标位置.
    Vector3 newPosition;

    public void Awake()
    {
        //添加CanvasGroup组件用于在拖拽是忽略自己，从而检测到被交换的图片.
        cg = this.gameObject.AddComponent<CanvasGroup>();
        rt = this.GetComponent<RectTransform>();

        tempParent = GameObject.Find("Canvas").transform.Find("Layer 2");
        Draging = false;

    }
    public void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(CanDrag);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }
        GameObject target = eventData.pointerEnter;
        if (target.transform.parent.name.Contains("ItemUI"))
        {
            if (target.GetComponent<DragOnPic>().itemPile != null)
            {
                UIManager.Instance.Open("ItemBreifUI", 1, args: new string[] {
                target.GetComponent<DragOnPic>().itemPile.item.ItemName,
                target.GetComponent<DragOnPic>().itemPile.item.ItemDescription
                + (UIManager.Instance.FindUI("ShopCargoUI")?"\n\n价格：" + target.GetComponent<DragOnPic>().itemPile.item.ItemPrice:"")
            });
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }
        GameObject target = eventData.pointerEnter;
        if (target.transform.parent.name.Contains("ItemUI"))
        {
            UIManager.Instance.Close("ItemBreifUI");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }
        if(othercargo == null)
        {
            return;
        }
        GameMgr.Get<IItemManager>().RemoveItemToCargo(PosID, 1, cargo, othercargo);
        GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        AudioControl.Instance.PlaySound("Tik");
        if (!CanDrag)
        {
            UIManager.Instance.Close("ItemBreifUI");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }

        Draging = true;
        AudioControl.Instance.PlaySound("Tik");

        //拖拽开始时记下自己的父物体.
        myParent = transform.parent;

        //拖拽开始时禁用检测.
        cg.blocksRaycasts = false;

        this.transform.SetParent(tempParent);
    }

    /// <summary>
    /// Raises the drag event.
    /// </summary>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }
        UIManager.Instance.Close("ItemBreifUI");
        //推拽是图片跟随鼠标移动.
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPosition);
        transform.position = newPosition;
    }

    /// <summary>
    /// Raises the end drag event.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != 0)
        {
            return;
        }
        if (!CanDrag)
        {
            return;
        }
        if (!Draging)
        {
            return;
        }
        Draging = false;
        UIManager.Instance.Close("ItemBreifUI");
        this.transform.SetParent(myParent);
        this.transform.localPosition = Vector3.zero;
        //拖拽结束时启用检测.
        cg.blocksRaycasts = true;

        if (eventData.pointerEnter == null)
        {
            return;
        }

        //获取鼠标下面的物体.
        GameObject target = eventData.pointerEnter;

        //如果能检测到物体.
        if (target.transform.parent.name.Contains("ItemUI"))
        {
            AudioControl.Instance.PlaySound("Tik");

            //如果检测到图片，则触发交换效果
            if (target.GetComponent<DragOnPic>().itemPile == null)
            {
                GameMgr.Get<IItemManager>().SwitchPile(cargo, itemPile, target.GetComponent<DragOnPic>().cargo, target.GetComponent<DragOnPic>().PosID);
            }
            else
            {
                GameMgr.Get<IItemManager>().SwitchPile(cargo, itemPile, target.GetComponent<DragOnPic>().cargo, target.GetComponent<DragOnPic>().itemPile);
            }
            GameMgr.Get<IItemManager>().RefreshAllCargoUI();
        }
    }



}