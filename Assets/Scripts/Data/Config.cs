using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Config:MonoBehaviour
{
    public static Config Instance;

    public TextAsset matJsonAsset;
    public TextAsset ionJsonAsset;
    public TextAsset equationTextAsset;
    public TextAsset ionRuleTextAsset;

    private List<Element> mats;
    private List<Element> ions;
    private List<Equation> equations;
    private List<Equation> ionRules;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 解析配置
    /// </summary>
    public void Init()
    {
        string matConfigJson = matJsonAsset.text;
        mats = LitJson.JsonMapper.ToObject<List<Element>>(matConfigJson);

        string ionConfigJson = ionJsonAsset.text;
        ions = LitJson.JsonMapper.ToObject<List<Element>>(ionConfigJson);

        string equationJson = equationTextAsset.text;
        equations = LitJson.JsonMapper.ToObject<List<Equation>>(equationJson);

        string ionRuleJson = ionRuleTextAsset.text;
        ionRules = LitJson.JsonMapper.ToObject<List<Equation>>(ionRuleJson);

        foreach (var e in equations)
        {
            e.ParseLeftRight();
        }

        foreach (var e in ionRules)
        {
            e.ParseLeftRight();
        }
    }

    /// <summary>
    /// 获取所有固体配置
    /// </summary>
    /// <returns></returns>
    public List<Element> GetSolidsConfig()
    {
        List<Element> results = new List<Element>();
        foreach (Element VARIABLE in mats)
        {
            if (VARIABLE.state == 1)
            {
                results.Add(VARIABLE.Copy());
            }
        }
        return results;
    }

    /// <summary>
    /// 获取所有液体配置
    /// </summary>
    /// <returns></returns>
    public List<Element> GetLiquidsConfig()
    {
        List<Element> results = new List<Element>();
        foreach (Element VARIABLE in mats)
        {
            if (VARIABLE.state == 2)
            {
                results.Add(VARIABLE.Copy());
            }
        }
        return results;
    }

    /// <summary>
    /// 根据id获取物质/离子配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Element GetElement(int id)
    {
        foreach (Element VARIABLE in mats)
        {
            if (VARIABLE.id == id)
            {
                return VARIABLE.Copy();
            }
        }

        foreach (Element VARIABLE in ions)
        {
            if (VARIABLE.id == id)
            {
                return VARIABLE.Copy();
            }
        }

        return null;
    }

    /// <summary>
    /// 获取物质电离产生结果
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public List<Element> GetIonResult(Element e)
    {
        List<Element> results = new List<Element>();
        foreach (Equation equation in ionRules)
        {
            if (equation.leftElements[0].id == e.id)
            {
                for (int i = 0; i < equation.rightElements.Count; i++)
                {
                    results.Add(GetElement(equation.rightElements[i].id));
                }
                return results;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据输入的物质和离子列表，返回匹配的反应方程列表
    /// </summary>
    /// <param name="elements"></param>
    /// <returns></returns>
    public List<Equation> GetEquationMatched(List<Element> elements)
    {
        List<Equation> results = new List<Equation>();
        foreach (Equation equation in equations)
        {
            if (equation.TestElements(elements))
            {
                results.Add(equation.Copy());
            }
        }
        return results;
    } 
}
