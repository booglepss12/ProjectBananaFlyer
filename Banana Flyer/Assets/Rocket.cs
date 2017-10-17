using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
   
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 2f;
    Rigidbody rigidBody;
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource= GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
          
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying) // so no weird audio artifacts
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }

        }
    }
     void OnCollisionEnter(Collision collision)
    {
       switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("Safe"); //TODO remove this line
                break;
            case "Deadly":
                print("You are dead"); //TODO remove this line
                break;
        }

    }
}
