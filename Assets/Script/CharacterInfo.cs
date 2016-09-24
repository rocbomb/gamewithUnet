using UnityEngine;
using System.Collections;

public class CharacterInfo : MonoBehaviour {

    public int CharID = 0;

    public Sprite[] sprites = new Sprite[10];

    public SpriteRenderer sr;

	// Use this for initialization
	void Awake () {
	
	}

    public void setmaninfo(int id)
    {
        sr.sprite = sprites[id];
        CharID = id + 1;
    }
}
