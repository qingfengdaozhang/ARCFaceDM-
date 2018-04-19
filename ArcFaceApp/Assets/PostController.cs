using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PostController : MonoBehaviour
{
    public static PostController Instance { set; get; }
    public Image image;
    public byte[] imgBytes;
    // Use this for initialization
    string url = "";
    public Text login_name;
    public Text login_password;
    public Text register_name;
    public Text register_password;
    public Text register_passwordenter;
    public Text register_housename;
    public Text register_faceinfoid;
    public Text alert_text;
    public Text visitorname;
    public GameObject[] visitorlist;
    class ResInfo
    {
        public string result;
        public string username;
        public string password;
        public string housename;
        public string faceinfoid;
        public int id;
    }
    class Visitor
    {
        public string visitorname;
        public string householderid;
        public byte[] image;
    }
    class Result
    {
        public string result;
    }
    private ResInfo mainInfo = new ResInfo();
    void Start()
    {
        Instance = this;
    }

    public void cleanVisitorlist()
    {
        foreach(GameObject child in visitorlist)
        {
            child.SetActive(false);
        }
    }

    public void login()
    {
        StartCoroutine(PostLogin());
    }


    public void register()
    {
        if (register_password.text.Equals(register_passwordenter.text))
        {
            StartCoroutine(PostRegister());
        }
    }

    public void addvisitor()
    {
        StartCoroutine(PostAddVisitor());
    }

    public void getvisitorbyid()
    {
        StartCoroutine(GetVisitorByID());
    }

    IEnumerator PostLogin()
    {
        Debug.Log("开始发送");
        url = "http://120.78.141.179:80/account/applogin";
        JsonData data = new JsonData();
        data["username"] = login_name.text;
        data["password"] = login_password.text;
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, postBytes, header);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            UIController.Instance.OnAlert();
            alert_text.text = "网络错误" + www.error;
        }
        else
        {
            ResInfo r = JsonMapper.ToObject<ResInfo>(www.text);
            if (r.result.Equals("success"))
            {
                UIController.Instance.OnMenu();
                UIController.Instance.OnAlert();
                mainInfo = r;
                getvisitorbyid();
                alert_text.text = "登录成功";
            }
            else
            {
                UIController.Instance.OnAlert();
                alert_text.text = "登录失败";
            }
        }
    }
    IEnumerator PostRegister()
    {
        url = "http://120.78.141.179:80/account/appregister";
        JsonData data = new JsonData();
        data["username"] = register_name.text;
        data["password"] = register_password.text;
        data["housename"] = register_housename.text;
        data["faceinfoid"] = register_faceinfoid.text;
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, postBytes, header);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            UIController.Instance.OnAlert();
            alert_text.text = "网络错误" + www.error;
        }
        else
        {
            ResInfo r = JsonMapper.ToObject<ResInfo>(www.text);
            if (r.result.Equals("success"))
            {
                UIController.Instance.OnAlert();
                alert_text.text = "注册成功";

            }
            else
            {
                UIController.Instance.OnAlert();
                alert_text.text = "注册失败";
            }
        }



    }
    IEnumerator PostAddVisitor()
    {
        Debug.Log("开始发送");
        url = "http://120.78.141.179:80/account/addvisitor";
        if (imgBytes == null || visitorname.text.Equals(""))
        {
            UIController.Instance.OnAlert();
            alert_text.text = "请选择图片并且访客名称不能为空";
        }

        WWWForm form = new WWWForm();
        form.AddField("visitorname", visitorname.text);
        form.AddField("householderid", mainInfo.id.ToString());
        form.AddBinaryData("image", imgBytes);
        WWW www = new WWW(url, form);

        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            UIController.Instance.OnAlert();
            alert_text.text = "网络错误" + www.error;
        }
        else
        {

            Result r = JsonMapper.ToObject<Result>(www.text);
            Debug.Log(r.result);
            if (r.result.Equals("success"))
            {
                UIController.Instance.OnMenu();
                UIController.Instance.OnAlert();
                alert_text.text = "访客增加成功";
            }
            else
            {
                UIController.Instance.OnAlert();
                alert_text.text = "访客增加失败";
            }
        }
    }
    IEnumerator GetVisitorByID()
    {
        url = "http://120.78.141.179:80/account/getvisitorbyhid";
        JsonData data = new JsonData();
        Debug.Log("ouseholderid" + mainInfo.id);
        data["householderid"] = mainInfo.id.ToString();
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, postBytes, header);
        cleanVisitorlist();
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            UIController.Instance.OnAlert();
            alert_text.text = "网络错误" + www.error;
        }
        else
        {
            JsonData jd = JsonMapper.ToObject(www.text);
            Debug.Log(www.text);
            for(int i = 0; i < jd.Count; i++)
            {
                JsonData js = jd[i]["fields"];
                visitorlist[i].SetActive(true);
                visitorlist[i].GetComponent<VisitorInfo>().name.text = js["visitorname"].ToString();
                if (js["isfaceinfoid"].ToString().Equals("1"))
                    visitorlist[i].GetComponent<VisitorInfo>().isface.text = "否";
                else
                    visitorlist[i].GetComponent<VisitorInfo>().isface.text = "是";
            }
        }

    }
}
