using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
   
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 2f;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject successVFX;

    //required component references
    Rigidbody rigidBody;
    AudioSource audioSource;
    float timerStart;
    bool sceneIsLoading = false;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource= GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timerStart <= Mathf.Epsilon) // timer not set and process input
        {
            Rotate();
            Thrust();
        }
        else
        {
            LoadSceneAfterSeconds(2.0f);
        }
    }

    private void LoadSceneAfterSeconds(float seconds)
    {
        if (!sceneIsLoading) { return; }
        sceneIsLoading = true;
        print("I'll load the scene later");
        
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
                audioSource.PlayOneShot(mainEngine);
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
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Deadly":
                StartDeathSequence();
                break;
        }

    }

    private void StartSuccessSequence()
    {
        if (timerStart > Mathf.Epsilon) { return; } //don't bother if timer set
        //make some noise
        audioSource.PlayOneShot(success);
        //stop engine noise
        audioSource.Stop();
        // show some fireworks
        successVFX.SetActive(true);
        timerStart = Time.time; //carry on later
        //starting success sequence
    }

    private void StartDeathSequence()
    {
        if (timerStart > Mathf.Epsilon) { return; } //don't bother if timer set
        //make some noise
        
        //stop engine noise
        audioSource.Stop();
        // show some sparks
        audioSource.PlayOneShot(explosion);
        deathVFX.SetActive(true);

        timerStart = Time.time; //carry on later
        //starting death sequence
        

    }
}
