using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class Rocket : MonoBehaviour {
   
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 2f;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem engineExhaust;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] ParticleSystem successVFX;
    [SerializeField] float levelLoadDelay = 2f;
    

    //required component references
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    float timerStart;
    bool sceneIsLoading = false;
    bool collisionsAreEnabled = true;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource= GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive) // timer not set and process input
        {
            RepondToRotateInput();
            RespondToThrustInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //toggle collisions
            collisionsAreEnabled = !collisionsAreEnabled;
        }
    }

    private void RepondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; //remove rotation due to physics
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
          
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
      
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        engineExhaust.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying) // so no weird audio artifacts
        {
            audioSource.PlayOneShot(mainEngine);
        }
        engineExhaust.Play();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionsAreEnabled) { return;}
        switch (collision.gameObject.tag)
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
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successVFX.Play();
        sceneIsLoading = true;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
        deathVFX.Play();
        Invoke("LoadSameLevel", levelLoadDelay);
    }

    private void LoadSameLevel()
    {
        deathVFX.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;
        int thisSceneIndex = (currentSceneIndex) % totalNumberOfScenes;
        SceneManager.LoadScene(thisSceneIndex); 
    }

    private void LoadNextScene()
    {
        successVFX.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalNumberOfScenes;
        SceneManager.LoadScene(nextSceneIndex); 
    }

    
}
