using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed;

    private GameObject Child;

	// Use this for initialization
	void Start () {
        Child = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        //Get X and Y movements
        float x = Input.GetAxis("Horizontal"), y = Input.GetAxis("Vertical");
        //Move the player with our specified input
        transform.Translate(new Vector3(x,y,0f)*moveSpeed/Variables.speedDampener);

        //Calculate the rotation we're traveling in
        float atan = Mathf.Atan2(y,x)*Mathf.Rad2Deg;

        //Incredibly hacky way to rotate the Object without messing up the relative Translate
        //Will have to find a better solution
        if( Mathf.Abs(x) > 0.05 || Mathf.Abs(y) > 0.05)
            Child.transform.rotation = Quaternion.Euler(0f,0f,atan-90);
    }
}
