using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBrain : Dog {

    public float wanderTimer;
    public Animator animator;
    float move;
    
    bool wandering;

	// Use this for initialization
	void Start ()
    {
        GetGridObject();
        

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            

        wanderTimer += Time.deltaTime;
        if ((wanderTimer >= 5 || wanderTimer <= 6f) && isLerping == false)
        {
            Wandering();
        }
	}

    void Wandering()
    {
        wanderTimer = 0;

        Vector3 currentPos = transform.position;
        Node currentNode = new Node();
        currentNode.coord = currentPos;
        WanderLerp(currentNode, gridScript.GetRandomNode());      
        animator.SetFloat("Move", 0.6f);
    }
}
