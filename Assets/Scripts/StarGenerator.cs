using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour {

    public GameObject starPrefab;

    Color[] colors = new Color[] {
        Color.red, Color.blue, Color.white, new Color(255,165,0), Color.yellow, Color.yellow
    };

	// Use this for initialization
	void Start () {
		
        for(int i = 0; i < 1000; i++) {
            float x = Random.Range(-30f,140f),y = Random.Range(-80f,80f);
            GameObject star = Instantiate<GameObject>(starPrefab);
            star.transform.position = new Vector3(x,y,0f);
            star.transform.parent = transform;
            star.GetComponent<SpriteRenderer>().color = colors[(int)Random.Range(0,colors.Length)];
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
