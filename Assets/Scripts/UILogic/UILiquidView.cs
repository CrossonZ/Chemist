using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILiquidView : MonoBehaviour {

    public GameObject item;
    bool dragging = false;
    public GameObject water;
    public GameObject Arrow;

    void Start()
    {
        List<Element> solids = Config.Instance.GetLiquidsConfig();

        foreach (var VARIABLE in solids)
        {
            GameObject uiItem = Instantiate(item);
            uiItem.transform.SetParent(transform);
            uiItem.transform.localScale = Vector3.one;
            uiItem.transform.localPosition = Vector3.zero;
            uiItem.transform.localRotation = Quaternion.identity;
            RectTransform rect = uiItem.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector3.zero;
                rect.sizeDelta = Vector3.zero;
            }
            uiItem.GetComponent<ItemCtrl>().element = VARIABLE;
            uiItem.GetComponent<ItemCtrl>().text.text = VARIABLE.name;
            UIEventListener.Get(uiItem).onDown = OnPointerDown;
            UIEventListener.Get(uiItem).onUp = OnPointerUp;
        }
    }

    void OnPointerDown(GameObject go)
    {
        if (dragging) return;
        dragging = true;

    }

    void OnPointerUp(GameObject go)
    {
        if(Arrow.activeSelf) MainCtrl.Instance.AddElement(go.GetComponent<ItemCtrl>().element.Copy());

        dragging = false;
        Arrow.SetActive(false);
        water.transform.position = new Vector3(0, 10, 8.5f);
        water.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (water == null) return;
        if (!dragging) return;
        if(dragging) water.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,3));

        //water.transform.position = water.transform.position + new Vector3(0, 0, -1 * water.transform.position.z + 10);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，在scene视图中能看到由摄像机发射出的射线
            GameObject gameObj = hitInfo.collider.gameObject;
            if (gameObj.name.StartsWith("Cup") == true)//当射线碰撞目标的name包含Cube，执行拾取操作
            {
                //Debug.Log(gameObj.name);
                water.transform.eulerAngles = new Vector3(0, 0, -80);
                Arrow.SetActive(true);
                Vector3 arrowPos = Camera.main.WorldToScreenPoint(gameObj.transform.position);
                arrowPos.y = Input.mousePosition.y;
                arrowPos.x = arrowPos.x - 150;
                Arrow.transform.position = arrowPos;
                //gameObj.transform.FindChild("fill").gameObject.SetActive(true);
                //gameObj.transform.FindChild("fill").localScale = new Vector3(1,0.4f,1);
            }
            else
            {
                Arrow.SetActive(false);
                water.transform.eulerAngles = new Vector3(0, 0,0);
            }
        }
        else
        {
            Arrow.SetActive(false);
            water.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }
}
