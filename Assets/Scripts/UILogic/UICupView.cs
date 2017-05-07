using UnityEngine;
using System.Collections;

public class UICupView : MonoBehaviour {

    public GameObject cup1;
    bool dragging = false;
    public GameObject cupPrefab;
    GameObject cup;

    void Start()
    {
        UIEventListener.Get(cup1).onDown = OnPointerDown;
        UIEventListener.Get(cup1).onUp = OnPointerUp;
    }

    void OnPointerDown(GameObject go)
    {
        Debug.Log("down");
        if (dragging) return;
        cup = Instantiate(cupPrefab);
        dragging = true;

    }

    void OnPointerUp(GameObject go)
    {
        dragging = false;
        MainCtrl.Instance.SetContainer(cup);
        Debug.Log("up");
    }

    void Update()
    {
        if (cup == null) return;
        if (!dragging) return;
        if(dragging) cup.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,3));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，在scene视图中能看到由摄像机发射出的射线
            GameObject gameObj = hitInfo.collider.gameObject;
            if (gameObj.name.StartsWith("table") == true)//当射线碰撞目标的name包含Cube，执行拾取操作
            {
                //Debug.Log(gameObj.name);
                //cup.transform.position = hitInfo.point + new Vector3(0,0.6f,0);
            }
        }

    }
}
