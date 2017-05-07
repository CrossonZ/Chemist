using UnityEngine;
using System.Collections;

public class CupCtrl : MonoBehaviour {

    public GameObject water;
    public MonoBehaviour uvScroll;
    public GameObject effect;
    public GameObject effectGas;
    public GameObject solid;

    void Start()
    {

    }

    public void ShowWater(float value)
    {
        water.SetActive(true);
        water.transform.localScale = new Vector3(water.transform.localScale.x,value, water.transform.localScale.y);
    }

    public void HideWater()
    {
        water.SetActive(false);
    }

    public void ShowSolid(float value)
    {
        solid.SetActive(true);
        solid.transform.localScale = new Vector3(water.transform.localScale.x, value, water.transform.localScale.y);
    }

    public void HideSolid()
    {
        water.SetActive(false);
    }

    public void ShowEffect(int fxType = 1)
    {
        switch (fxType)
        {
            case 1:
                effect.SetActive(true);
                break;
            case 2:
                effectGas.SetActive(true);
                break;

            default:
                break;
        }
        
    }

    public void HideEffect()
    {
        effect.SetActive(false);
    }
}
