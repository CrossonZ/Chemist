using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Equation
{
    public string left;
    public string right;
    public List<Element> leftElements;
    public List<Element> rightElements;
    public string equation;

    public void ParseLeftRight()
    {
        leftElements = new List<Element>();
        rightElements = new List<Element>();

        string[] ll = left.Split('|');
        for (int i = 0; i < ll.Length; i++)
        {
            string[] args = ll[i].Split(',');
            float amount = float.Parse(args[0]);
            int id = int.Parse(args[1]);
            Element e = Config.Instance.GetElement(id);
            e.amount = amount;
            leftElements.Add(e);
        }

        string[] rr = right.Split('|');
        for (int i = 0; i < rr.Length; i++)
        {
            string[] args = rr[i].Split(',');
            float amount = float.Parse(args[0]);
            int id = int.Parse(args[1]);
            Element e = Config.Instance.GetElement(id);
            e.amount = amount;
            rightElements.Add(e);
        }
    }

    public bool TestElements(List<Element> elements)
    {
        int matchCnt = 0;
        foreach (Element need in leftElements)
        {
            bool find = false;
            foreach (Element give in elements)
            {
                if (need.id == give.id)
                {
                    find = true;
                    matchCnt++;
                }
            }
            if (!find)
            {
                return false;
            }
        }
        return matchCnt == leftElements.Count;
    }

    public Equation Copy()
    {
        Equation e = new Equation() {left = left, right = right, equation = equation};
        e.ParseLeftRight();
        return e;
    }
}
