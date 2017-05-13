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

    /// <summary>
    /// 之前烧杯里的总物质列表
    /// </summary>
    private List<Element> m_oldElements;

    List<Equation> m_oldEquations ;


    // Use this for initialization
    void Start ()
	{
	    Instance = this;
        Config.Instance.Init();
        m_elements = new List<Element>();
        m_oldElements = new List<Element>();
        m_oldEquations = new List<Equation>();
    } 

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (m_oldElements.Equals(m_elements))
            return;
        if (m_elements.Count == 0) 
            return;
        m_oldElements.Clear();
        foreach (Element e in m_elements)
        {
            m_oldElements.Add(e);
        }
        //匹配可反应式子
        OnModify();

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
            //e.count = 1000;
            m_elements.Add(e);
            //新增物质/离子后，容器内的结果计算
            foreach (Element ele in OnAddNewElement(e))
            {
                m_elements.Add(ele);
            }
            //List<Element> addElement = new List<Element>();
            //addElement.Add(e);
            //匹配可反应式子
            //OnModify();
        }
        else
        {
            storedElement.count += e.amount;
        }
        OnModify();
    }

    private List<Element> OnAddNewElement(Element e)
    {
        List<Element> toAdds = new List<Element>();
        for (int i = 0; i < m_elements.Count; i++)
        {
            if (m_elements[i].ionization == 1)
            {
                List<Element> ionResults = Config.Instance.GetIonResult(m_elements[i]);
                foreach (Element VARIABLE in ionResults)
                {
                    if(m_elements.Find(item => item.id == VARIABLE.id) ==null)
                    toAdds.Add(VARIABLE.Copy());
                }
            }
        }

        return toAdds;

        //for (int i = 0; i < toAdds.Count; i++)
        //{
        //    AddElement(toAdds[i]);
        //}
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

        foreach (Equation equation in equations)
        {
            foreach (Element element in equation.leftElements)
            {
                List<Element> m_beRemoveElements = new List<Element>();
                foreach (Element ele in m_elements)
                {
                    if (ele.id == element.id)
                    {
                        ele.count -= element.amount;
                        if (ele.count == 0)
                        {
                            m_beRemoveElements.Add(ele);
                        }
                        else if (ele.count < element.amount)
                        {
                            m_beRemoveElements.Add(ele);
                        }
                    }
                }
                foreach (Element ele in m_beRemoveElements)
                {
                    m_elements.Remove(ele);
                }
                
            }

            foreach (Element element in equation.rightElements)
            {
                Element storedElement = m_elements.Find(item => item.id == element.id);
                if (storedElement != null)
                {
                    storedElement.count += element.amount;
                }
                else
                {
                    element.count = element.amount;
                    m_elements.Add(element);
                }
            }
        }

        if (equations != null && equations.Count > 0 && !equations.Equals(m_oldEquations))
        {
            for (int i = 0; i < equations.Count; i++)
            {
                if (m_oldEquations.Count == 0)
                {
                    m_oldEquations.Add(equations[i]);
                    uiFunction.AddFunction(equations[i].equation);
                    continue;
                }
                List<Equation> m_beAddElements = new List<Equation>();
                if (!isEquationExist(m_oldEquations, equations[i]))
                {
                    uiFunction.AddFunction(equations[i].equation);
                    m_beAddElements.Add(equations[i]);
                }

                foreach (Equation ele in m_beAddElements)
                {
                    m_oldEquations.Add(ele);
                }

            }
            if (m_oldEquations.Count != 0)
            {
                //ui 现实 可放映的方程
                uiFunction.gameObject.SetActive(true);
            }
            else
            {
                uiFunction.gameObject.SetActive(false);
            }
            //把反应特效打开
            //根据反应方程配置的效果类型，打开不同的特效，现在只有写死的一个
            //cupCtrl.ShowEffect(1);
            cupCtrl.ShowEffect();
        }
        else
        {
            if (cupCtrl != null)
            {
                ///没有匹配到方程，关闭所有效果
                cupCtrl.HideEffect();
                //uiFunction.gameObject.SetActive(false);
            }
        }

        uiDataTable.gameObject.SetActive(true);
        //刷新物质列表内容
        uiDataTable.Show(m_elements);
    }

    public bool isEquationExist(List<Equation> m_oldEquations, Equation e)
    {
        foreach (Equation equ in m_oldEquations)
        {
            if (equ.equation == e.equation)
                return true;
        }
        return false;
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
