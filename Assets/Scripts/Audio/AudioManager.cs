using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] public AudioClip bgm;
    [SerializeField] public AudioClip[] sliceSounds;
    [SerializeField] public AudioClip beachAmbiance;
    [SerializeField] public AudioClip timeOut;
    [SerializeField] public AudioClip knifeRain;
    [SerializeField] public AudioClip noKnife;

    [Header("Footstep Settings")]
    [SerializeField] public float minTimeBetweenslices = 0.3f;
    [SerializeField] public float maxTimeBetweenslices = 0.6f;

    private float timeSinceLastSlice; // Time since the last footstep sound
    private int sliceStepCount = 0;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(bgm);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopAllSFX()
    {
        sfxSource.Stop();
    }

    public void PlaySliceSFX()
    {
        // Check if enough time has passed to play the next footstep sound
        if (Time.time - timeSinceLastSlice >= Random.Range(minTimeBetweenslices, maxTimeBetweenslices))
        {
            AudioClip sliceStepSound = sliceSounds[Random.Range(0, sliceSounds.Length)];
            sfxSource.PlayOneShot(sliceStepSound);
            //sliceStepCount = (sliceStepCount + 1) % sliceSounds.Length;
            timeSinceLastSlice = Time.time; // Update the time since the last footstep sound
        }
    }
}