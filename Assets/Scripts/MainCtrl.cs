using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCtrl : MonoBehaviour
{
    public static MainCtrl Instance;

    private CupCtrl cupCtrl;

    [SerializeField]
    private UIFunction uiFunction;

    [SerializeField]
    private UIDataTable uiDataTable;

    /// <summary>
    /// 目前烧杯里的总物质列表
    /// </summary>
    private List<Element> m_elements;

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
        Config.Instance.Init();
        m_elements = new List<Element>();
    }

    /// <summary>
    /// 被操作驱动，添加物质/离子
    /// </summary>
    /// <param name="e"></param>
    public void AddElement(Element e)
    {
        if (cupCtrl == null)
        {
            Debug.Log("[Warning]Please select a container first!");
            return;
        }
        Element storedElement = m_elements.Find(item => item.id == e.id);
        if (storedElement == null)
        {
            m_elements.Add(e);
            //新增物质/离子后，容器内的结果计算
            OnAddNewElement(e);
            //匹配可反应式子
            OnModify();
        }
        else
        {
            storedElement.amount += e.amount;
        }
    }

    private void OnAddNewElement(Element e)
    {
        switch (e.state)
        {
            case 2:
                cupCtrl.ShowWater(0.4f);
                break;
            case 1:
                cupCtrl.ShowSolid(1f);
                break;
            default:
                break;
        }

        List<Element> toAdds = new List<Element>();
        for (int i = 0; i < m_elements.Count; i++)
        {
            if (m_elements[i].ionization == 1)
            {
                List<Element> ionResults = Config.Instance.GetIonResult(m_elements[i]);
                foreach (Element VARIABLE in ionResults)
                {
                    toAdds.Add(VARIABLE.Copy());
                }
            }
        }

        for (int i = 0; i < toAdds.Count; i++)
        {
            AddElement(toAdds[i]);
        }
    }

    public void ClearElements()
    {
        m_elements.Clear();
    }

    public void SetContainer(GameObject cupObj)
    {
        if (cupCtrl != null)
        {
            ClearElements();
            Destroy(cupObj);
        }
        cupCtrl = cupObj.GetComponent<CupCtrl>();
    }
    
    /// <summary>
    /// 物质种类发生变化
    /// </summary>
    public void OnModify()
    {
        //匹配可反应的方程
        List<Equation> equations = Config.Instance.GetEquationMatched(m_elements);
        if (equations != null && equations.Count > 0)
        {
            //ui 现实 可放映的方程
            uiFunction.gameObject.SetActive(true);
            uiFunction.AddFunction(equations[0].equation);

            //把反应特效打开
            //根据反应方程配置的效果类型，打开不同的特效，现在只有写死的一个
            //cupCtrl.ShowEffect(1);
            cupCtrl.ShowEffect();


        }
        else
        {
            ///没有匹配到方程，关闭所有效果
            cupCtrl.HideEffect();
            uiFunction.gameObject.SetActive(false);
        }

        uiDataTable.gameObject.SetActive(true);
        //刷新物质列表内容
        uiDataTable.Show(m_elements);
    }
    
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        
    }

    public void DebugList()
    {
        string ss = String.Empty;
        foreach (var VARIABLE in m_elements)
        {
            ss += "\n";
            ss += VARIABLE.name;
        }

        Debug.Log(ss);
    }
}
