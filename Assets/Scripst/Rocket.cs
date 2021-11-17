using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 120f;
    [SerializeField] float flySpeed = 800f;
    [SerializeField] AudioClip deadSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem deadParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] bool collisionStatus;
    [SerializeField] float fuel = 100f;
    [SerializeField] int fuelConsumption;
    [SerializeField] GameObject fuelProgressBar;

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State {Playing,Dead,Win};
    State state = State.Playing;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        collisionStatus = true;
    }


    void Update()
    {
        if(state == State.Playing)
        {
            Launch();
            Rotation();
            CheatKeys();
        }
    }
    void CheatKeys()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            collisionStatus = !collisionStatus;
        }
    }
    void Launch()
    {
        if (Input.GetKey(KeyCode.Space)&&fuel>0)
        {
            fuel -= fuelConsumption * Time.deltaTime;
            fuelProgressBar.transform.localScale = new Vector2(fuel/100,1);
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                flyParticles.Play();
            }
        }
        else
        {
            audioSource.Pause();
            flyParticles.Stop();
        }
    }
    
    void Rotation()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (state == State.Playing && collisionStatus)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    Finish();
                    break;
                default:
                    Dead();
                    break;
            }
        }
    }
    void OnTriggerEnter(Collider trigger)
    {
        if(trigger.gameObject.tag == "Fuel")
        {
            fuel += 50;
            fuelProgressBar.transform.localScale = new Vector2(fuel / 100, 1);
            Destroy(trigger.gameObject);
        }
    }

    void Finish()
    {
        state = State.Win;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextLevel", 3f);
    }
    void Dead()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        deadParticles.Play();
        Invoke("LoadCurrentLevel", 3f);
    }
    void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);
            state = State.Playing;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            state = State.Playing;
        }
    }
    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        state = State.Playing;
    }
}
