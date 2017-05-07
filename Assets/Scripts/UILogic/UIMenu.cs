using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour {

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    public GameObject containerView;
    public GameObject solidView;
    public GameObject liquidView;
    public GameObject gasView;

    // Use this for initialization
    void Start () {
        UIEventListener.Get(btn1.gameObject).onClick = OpenContainerView;
        UIEventListener.Get(btn2.gameObject).onClick = OpenSolidView;
        UIEventListener.Get(btn3.gameObject).onClick = OpenLiquidView;
        UIEventListener.Get(btn4.gameObject).onClick = OpenGasView;
    }
	
	void OpenContainerView(GameObject go)
    {
        containerView.SetActive(true);
        solidView.SetActive(false);
        liquidView.SetActive(false);
        gasView.SetActive(false);
    }

    void OpenSolidView(GameObject go)
    {
        containerView.SetActive(false);
        solidView.SetActive(true);
        liquidView.SetActive(false);
        gasView.SetActive(false);
    }

    void OpenLiquidView(GameObject go)
    {
        containerView.SetActive(false);
        solidView.SetActive(false);
        liquidView.SetActive(true);
        gasView.SetActive(false);
    }

    void OpenGasView(GameObject go)
    {
        containerView.SetActive(false);
        solidView.SetActive(false);
        liquidView.SetActive(false);
        gasView.SetActive(true);
    }
}
