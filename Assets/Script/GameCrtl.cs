using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class GameCrtl : MonoBehaviour {
    public Button exit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
