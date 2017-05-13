using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UILiquidView : MonoBehaviour {

    public GameObject item;
    bool dragging = false;
    public GameObject water;
    public GameObject Arrow;
    public GameObject ShowNumber;
    Transform tBaseUnit;
    Transform tMolUnit;
    float show = 0;
    ////add for 2d
    //Transform waterParent;

    void Start()
    {
        tBaseUnit = transform.parent.Find("Add/Base/Data");
        tMolUnit = transform.parent.Find("Add/Mol/Data");
        ////add for 2d
        //waterParent = GameObject.Find("Canvas").transform;

        List<Element> solids = Config.Instance.GetLiquidsConfig();
        ////add for 2d
        //Transform tParent = transform.FindChild("Viewport/Content");

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
        ////add for 2d
        //water = Instantiate(go);
        //water.transform.SetParent(waterParent);
        //water.transform.localScale = Vector3.one;
        //water.transform.localPosition = Vector3.zero;
        //water.transform.localRotation = Quaternion.identity;

    }

    void OnPointerUp(GameObject go)
    {
        if (Arrow.activeSelf)
        {
            Element beAddEle = go.GetComponent<ItemCtrl>().element.Copy();
            beAddEle.count = show;
            MainCtrl.Instance.AddElement(beAddEle);
        } 

        dragging = false;
        Arrow.SetActive(false);
        ShowNumber.gameObject.SetActive(false);
        ////add for 2d
        //Destroy(water);
        //delete for 2d
        water.transform.position = new Vector3(0, 10, 8.5f);
        water.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (water == null) return;
        if (!dragging) return;
        //change for 2d
        if (dragging)
            //water.transform.position = Input.mousePosition;
        if (dragging) water.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,3));

        //water.transform.position = water.transform.position + new Vector3(0, 0, -1 * water.transform.position.z + 10);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，在scene视图中能看到由摄像机发射出的射线
            GameObject gameObj = hitInfo.collider.gameObject;
            if (gameObj.name.StartsWith("Cup") == true)//当射线碰撞目标的name包含Cube，执行拾取操作
            {
                //Vector3 vObjScale = gameObj.transform.localScale;
                float sAllY = gameObj.GetComponent<BoxCollider>().size.y;

                float fFingerPosY = hitInfo.point.y - gameObj.transform.position.y + gameObj.GetComponent<BoxCollider>().size.y/2;
                Debug.Log("sAllY:" + sAllY + ";  fFingerPosY" + fFingerPosY + ";  gameObjY " + gameObj.transform.position.y);
                show = 300 * fFingerPosY / sAllY;
                //Debug.Log(gameObj.name);
                water.transform.eulerAngles = new Vector3(0, 0, -80);
                Arrow.SetActive(true);
                Vector3 arrowPos = Camera.main.WorldToScreenPoint(gameObj.transform.position);
                arrowPos.y = Input.mousePosition.y;
                arrowPos.x = arrowPos.x - 150;
                Arrow.transform.position = arrowPos;
                //gameObj.transform.FindChild("fill").gameObject.SetActive(true);
                //gameObj.transform.FindChild("fill").localScale = new Vector3(1,0.4f,1);

                ShowNumber.gameObject.SetActive(true);
                ShowNumber.transform.position = Input.mousePosition + new Vector3(-80, 60, 3);
                tBaseUnit.GetComponent<Text>().text = show.ToString();
            }
            else
            {
                Arrow.SetActive(false);
                water.transform.eulerAngles = new Vector3(0, 0,0);
                ShowNumber.gameObject.SetActive(false);
            }
        }
        else
        {
            Arrow.SetActive(false);
            water.transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }
}
