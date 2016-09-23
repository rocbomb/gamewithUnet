using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScoreMessage : MessageBase
{
    public int clinetid;
    public bool facedirection;
    public Vector3 scorePos;
    public int lives;
}

public class SuperPlayerCtrl : NetworkBehaviour {


    Rigidbody2D rigibody;

    JoyStickerController joystick;
	// Use this for initialization
	void Awake () {
		GetComponent<SpriteRenderer>().color = Color.red;
        rigibody = GetComponent<Rigidbody2D>();
        joystick = GameObject.Find("Joystick").GetComponent<JoyStickerController>();
    }

    public GameObject bulletPref;
    public Transform bulletSpawn;


    public float speed = 10;
    public float MaxSpeedY = 10;

    public float MaxSpeedX = 10;

    bool LeftKey = false;
    bool RightKey = false;
    bool UpKey = false;
    bool DownKey = false;

    bool oldfaceDirect = true;
    bool faceDirect = true;

    NetworkClient myclient;
    public float angle = 0;

    int IDCODE = 0;

    private void Start()
    {
        myclient = GameManager.Instance.myclient;
        if(!isLocalPlayer)
            myclient.RegisterHandler(100, OnChangePos);
    }
    public void OnChangePos(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("OnScoreMessage " + msg.clinetid);
        if (GameManager.Instance.ID != msg.clinetid)
            changeScale(msg.facedirection);
    }
    private void changeScale(bool flag)
    {
        if(flag)
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, transform.localScale.y);
        else
            transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.y);

    }

    void Update () {

        if (!isLocalPlayer)
            return;

        //Debug.Log(joystick.InputDirection);
       // var k = (joystick.InputDirection.y / joystick.InputDirection.x);
        angle = Mathf.Atan2(joystick.InputDirection.z * 100, joystick.InputDirection.x * 100) * Mathf.Rad2Deg;
        //Debug.Log(angle * Mathf.Rad2Deg);

        if (angle < 60 && angle > -60)
            RightKey = true;
        else
            RightKey = false;
        if (angle > 30 && angle < 150)
            UpKey = true;
        else
            UpKey = false;
        if (angle > -150 && angle < -60)
            DownKey = true;
        else
            DownKey = false;
        if (angle > 120 || angle < -120)
            LeftKey = true;
        else
            LeftKey = false;

        if (joystick.InputDirection.magnitude < 0.1)
        {
            RightKey = false;
            LeftKey = false;
            UpKey = false;
            DownKey = false;
        }

        if ((LeftKey ||Input.GetKey(KeyCode.A)) && rigibody.velocity.x > -MaxSpeedY)
        {
            faceDirect = false;

            rigibody.AddForce(new Vector2(-50, 0), ForceMode2D.Force);
            if(rigibody.velocity.x > 0)
                rigibody.velocity = new Vector2(0, rigibody.velocity.y);
        }

        if ((RightKey || Input.GetKey(KeyCode.D)) && rigibody.velocity.x < MaxSpeedY)
        {
            faceDirect = true;

            rigibody.AddForce(new Vector2(50, 0), ForceMode2D.Force);
            if (rigibody.velocity.x < 0)
                rigibody.velocity = new Vector2(0, rigibody.velocity.y);
        }

        if ((UpKey || Input.GetKey(KeyCode.W)) && rigibody.velocity.y < MaxSpeedY)
        {
            if (rigibody.velocity.y < 0)
            {
                rigibody.velocity = new Vector2(rigibody.velocity.x, 0);
            }
            rigibody.AddForce(new Vector2(0, 40), ForceMode2D.Force);
        }

        if ((DownKey || Input.GetKey(KeyCode.S)) && rigibody.velocity.y > -MaxSpeedY)
        {
            rigibody.AddForce(new Vector2(0, -30), ForceMode2D.Force);
        }

        if(faceDirect != oldfaceDirect)
        {
            oldfaceDirect = faceDirect;
            CmdChangeFace(GameManager.Instance.ID,faceDirect);
            changeScale(faceDirect);
        }



        //if (Input.GetKeyDown(KeyCode.Space))
        //    CmdChangeFace(false);
    }

    [Command]
    private void CmdChangeFace(int id,bool lv)
    {

        ScoreMessage msg = new ScoreMessage();
        msg.clinetid = id;
        msg.facedirection = lv;
        msg.scorePos = Vector3.zero;
        msg.lives = 0;
        Debug.Log("Server cmd");
        NetworkServer.SendToAll(100, msg);

        //GameObject bullet = (GameObject)Instantiate(bulletPref, bulletSpawn.position, bulletSpawn.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        //NetworkServer.Spawn(bullet);

        //Destroy(bullet, 2);
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}