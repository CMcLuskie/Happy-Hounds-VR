using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestyCuntScript : MonoBehaviour {

    [SerializeField]
    protected Animator animator;
	// Use this for initialization
	void Start () {
        animator.SetBool("Point", true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
