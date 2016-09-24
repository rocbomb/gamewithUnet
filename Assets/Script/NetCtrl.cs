using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetCtrl : NetworkBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ScoreMessage msg = new ScoreMessage();
        msg.clinetid = 33;
        msg.scorePos = Vector3.zero;
        msg.lives = 0;
        Debug.Log("Server cmd");
        //NetworkServer.SendToAll(100, msg);
    }
}
