using UnityEngine.SceneManagement;
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
    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    float timerStart;
    bool sceneIsLoading = false;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource= GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) // timer not set and process input
        {
            Rotate();
            Thrust();
        }
        
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
        if (state !== State.Alive) { return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 2f); //TODO paramertize time
                break;
            case "Deadly":
                state = State.Dying;
                LoadSameLevel();
                Invoke("LoadSameLevel", 2f); 
                break;
        }

    }

    private void LoadSameLevel()
    {
      
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); //TODO allow for more than two levels
    }

    
}
