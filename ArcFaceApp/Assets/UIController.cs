using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public static UIController Instance { set; get; }
    [SerializeField]
    private GameObject login;
    [SerializeField]
    private GameObject register;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject alert;
    [SerializeField]
    private GameObject addvisitor;
    private void Start()
    {
        Instance = this;
        login.SetActive(true);
        register.SetActive(false);
        menu.SetActive(false);
        alert.SetActive(false);
        addvisitor.SetActive(false);
    }
    public void OnLogin()
    {
        login.SetActive(true);
        register.SetActive(false);
        menu.SetActive(false);
        alert.SetActive(false);
        addvisitor.SetActive(false);
    }
    public void OnRegister()
    {
        login.SetActive(false);
        register.SetActive(true);
        menu.SetActive(false);
        alert.SetActive(false);
        addvisitor.SetActive(false);
    }
    public void OnMenu()
    {
        login.SetActive(false);
        register.SetActive(false);
        menu.SetActive(true);
        alert.SetActive(false);
        addvisitor.SetActive(false);

        PostController.Instance.getvisitorbyid();
    }

    public void OnAddvisitor()
    {
        login.SetActive(false);
        register.SetActive(false);
        menu.SetActive(false);
        alert.SetActive(false);
        addvisitor.SetActive(true);
    }


    public void OnAlert()
    {
        alert.SetActive(true);
    }
    public void OnAlertClose()
    {
        alert.SetActive(false);
    }

}
