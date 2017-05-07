using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIDataTable : MonoBehaviour
{
    public GameObject DataItem;
    public Transform container;
    public List<GameObject> items = new List<GameObject>(); 

	// Use this for initialization
	void Start () {
	
	}

    public void Show(List<Element> elements)
    {
        int newCnt = elements.Count - items.Count;

        for(int i=0;i<newCnt;i++)
        {
            NewItem();
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(false);
        }

        int index = 0;
        foreach (Element e in elements)
        {
            items[index].SetActive(true);
            items[index].GetComponent<Text>().text = e.name;
            index++;
        }
    }

    private void NewItem()
    {
        GameObject uiItem = Instantiate(DataItem);
        uiItem.transform.SetParent(container);
        uiItem.transform.localScale = Vector3.one;
        uiItem.transform.localPosition = Vector3.zero;
        uiItem.transform.localRotation = Quaternion.identity;
        RectTransform rect = uiItem.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector3.zero;
            rect.sizeDelta = Vector3.zero;
        }
        items.Add(uiItem);
    }
	
	
}
