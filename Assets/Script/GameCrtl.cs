using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class GameCrtl : MonoBehaviour {
    public Button exit;
    // Use this for initialization

    public NetCtrl netCtrl;

    public SuperPlayerCtrl self;
    public SuperPlayerCtrl enemy;

    /// <summary>
    /// 0 开始
    /// 1 等待另一个用户
    /// 2 开始选人
    /// 3 进入游戏
    /// </summary>
    public int state = 0;

    public GameObject waitforUI;
    public GameObject selectMan;

    public Button Select1;
    public Button Select2;


    void Awake () {
        Select1.onClick.AddListener(delegate () { ChoseCharacter(1); });
        Select2.onClick.AddListener(delegate () { ChoseCharacter(2); });

        if (GameManager.Instance.ID == 1)
        {
            state = 1;
            waitforUI.SetActive(true);
            waitforUI.transform.GetChild(1).GetComponent<Text>().text = LocalIPAddress();
        }
        if (GameManager.Instance.ID == 2)
        {
            state = 2;
            waitforUI.SetActive(false);
            selectMan.SetActive(true);
        }


    }
	
    private void ChoseCharacter(int index)
    {
        Debug.Log("send " + index);
        Transform[] select = new Transform[2];
        select[0] = Select1.transform;
        select[1] = Select2.transform;
        state = 3;

        selectMan.SetActive(false);

        for (int i=0; i<2;i++)
        {
            select[i].GetChild(1).gameObject.SetActive(i+1 == index);
        }
        self.CmdChoseCharacter(GameManager.Instance.ID, index);
    }


	// Update is called once per frame
	void Update () {
        if (self == null || enemy == null)
        {
            var go = GameObject.Find("self");
            if(go != null)
                self = go.GetComponent<SuperPlayerCtrl>();
            go = GameObject.Find("enemy");
            if (go != null)
            {
                enemy = go.GetComponent<SuperPlayerCtrl>();
                if (self.IDCODE == 1)
                    enemy.IDCODE = 2;
                else
                    enemy.IDCODE = 1;
            }
        }
	    if(state == 1 && GameManager.Instance.ID == 1) //主机
        {
            if (NetworkServer.connections.Count == 1)
                return;
            else
            {
                waitforUI.SetActive(false);
                state = 2;
                selectMan.SetActive(true);
            }
        }
	}

    public void ExitGame()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
        if (NetworkClient.active)
        {
            NetworkManager.singleton.StopClient();
        }
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }


    public void choseCharCallBack(int id,int charid)
    {
        if(id == GameManager.Instance.ID)
        {
            self.GetComponent<CharacterInfo>().setmaninfo(charid-1);
        }
        else
        {
            enemy.GetComponent<CharacterInfo>().setmaninfo(charid-1);
        }
    }
}
