using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public static AudioManager Instance;

    public AudioClip[] walkClips;
    public AudioClip[] runClips;
    public AudioSource jumpSound,collectSound,plantSound,interectSound, switchSound,clickSound, missionDoneSound;
    public AudioSource[] randomBackgroundSounds;
    public AudioSource plowingSound, throwingRodSound, AxeHitSound, noHittingSound, destroytreeSound,rodPullBackSound,doorSound,wateringSound;

    public AudioSource vehicleEngineStart,vehicleIsRunning;
    public AudioSource buySound, sellSound;
    public AudioSource vehicleEngineStartPlane, vehicleIsRunningPlane;
    public float walkVolume = 0.7f;
    public float runVolume = 1f;
    
    private void Start()
    {
        Instance = this;
        PlayRandomBackgroundSound();

        StartCoroutine(AnimalLoop(chickenSource, chickenClips));
        StartCoroutine(AnimalLoop(cowSource, cowClips));
        StartCoroutine(AnimalLoop(sheepSource, sheepClips));
        StartCoroutine(AnimalLoop(duckSource, duckClips));
        StartCoroutine(AnimalLoop(goatSource, goatClips));
    }

    private void Update()
    {
       DetectUIButton();
    }
    public void PlayWalkSound()
    {
        if (walkClips.Length == 0) return;

        int index = Random.Range(0, walkClips.Length);
        audioSource.PlayOneShot(walkClips[index], walkVolume);
    }

    public void PlayRunSound()
    {
        if (runClips.Length == 0) return;

        int index = Random.Range(0, runClips.Length);
        audioSource.PlayOneShot(runClips[index], runVolume);
    }
    public void playJumpSound()
    {
        if (!jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }
    public void playCollectSound()
    {
        if (!collectSound.isPlaying)
        {
            collectSound.Play();
        }
    }
    public void playPlantSound()
    {
        if (!plantSound.isPlaying)
        {
            plantSound.Play();
        }
    }
    public void playInterectSound()
    {
        if (!interectSound.isPlaying)
        {
            interectSound.Play();
        }
    }
    public void playSwitchSound()
    {
        if (!switchSound.isPlaying)
        {
            switchSound.Play();
        }
    }
    public void playClickSound()
    {
        if (!clickSound.isPlaying)
        {
            clickSound.Play();
        }
    }
    public void playMissionDoneSound()
    {
        if (!missionDoneSound.isPlaying)
        {
            missionDoneSound.Play();
        }
    }

    public void playPlowingSound()
    {
        if (!plowingSound.isPlaying)
        {
            plowingSound.Play();
        }
    }
    public void playThrowingRodSound()
    {
        if (!throwingRodSound.isPlaying)
        {
            throwingRodSound.Play();
        }
    }
    
    public void playAxeHitSound()
    {
        if (!AxeHitSound.isPlaying)
        {
            AxeHitSound.Play();
        }
    }

    public void playNoHittingSound()
    {
        if (!noHittingSound.isPlaying)
        {
            noHittingSound.Play();
        }
    }
    public void playDestroyTreeSound()
    {
        if (!destroytreeSound.isPlaying)
        {
            destroytreeSound.Play();
        }
    }
    public void playRodPullBackSound()
    {
        if (!rodPullBackSound.isPlaying)
        {
            rodPullBackSound.Play();
        }
    }

    public void playDoorSound()
    {
        if (!doorSound.isPlaying)
        {
                        doorSound.Play();
        }
    }
    public void playwateringSound()
    {
        if (!wateringSound.isPlaying)
        {
            wateringSound.Play();
        }

    }
    public void stopWateringSound()
    {
        if (wateringSound.isPlaying)
        {
            wateringSound.Stop();
        }
    }
    public void playBuySound()
    {
        if (!buySound.isPlaying)
        {
            buySound.Play();
        }
    }
    public void playSellSound()
    {
        if (!sellSound.isPlaying)
        {
            sellSound.Play();
        }
    }
    private Coroutine loopRoutineBGSound;

    public void PlayRandomBackgroundSound()
    {
        if (loopRoutineBGSound != null) return; // prevent multiple loops
        loopRoutineBGSound = StartCoroutine(RandomSoundLoop());
    }

    IEnumerator RandomSoundLoop()
    {
        while (true)
        {
            if (randomBackgroundSounds.Length == 0) yield break;

            int index = Random.Range(0, randomBackgroundSounds.Length);
            AudioSource source = randomBackgroundSounds[index];

            source.Play();

            // Wait until the audio finishes
            yield return new WaitForSeconds(source.clip.length);
        }
    }

    public void StopRandomBackgroundSound()
    {
        if (loopRoutineBGSound != null)
        {
            StopCoroutine(loopRoutineBGSound);
            loopRoutineBGSound = null;
        }
    }

    private Coroutine engineRoutineCar;

    public void StartVehicleEngine()
    {
        if (engineRoutineCar != null) return; // prevent double start
        engineRoutineCar = StartCoroutine(EngineStartSequence());
    }

    IEnumerator EngineStartSequence()
    {
        // Play engine start sound
        vehicleEngineStart.Play();

        // Wait until start sound finishes
        yield return new WaitForSeconds(vehicleEngineStart.clip.length-8);

        // Start running engine sound
        vehicleIsRunning.loop = true;
        vehicleIsRunning.Play();
    }

    public void StopVehicleEngine()
    {
        if (engineRoutineCar != null)
        {
            StopCoroutine(engineRoutineCar);
            engineRoutineCar = null;
        }

        vehicleEngineStart.Stop();
        vehicleIsRunning.Stop();
    }


    private Coroutine engineRoutinePlane;

    public void StartVehicleEnginePlane()
    {
        if (engineRoutinePlane != null) return; // prevent double start
        engineRoutinePlane = StartCoroutine(EngineStartSequencePlane());
    }

    IEnumerator EngineStartSequencePlane()
    {
        // Play engine start sound
        vehicleEngineStartPlane.Play();

        // Wait until start sound finishes
        yield return new WaitForSeconds(vehicleEngineStartPlane.clip.length-2);

        // Start running engine sound
        vehicleIsRunningPlane.loop = true;
        vehicleIsRunningPlane.Play();
    }

    public void StopVehicleEnginePlane()
    {
        if (engineRoutinePlane != null)
        {
            StopCoroutine(engineRoutinePlane);
            engineRoutinePlane = null;
        }

        vehicleEngineStartPlane.Stop();
        vehicleIsRunningPlane.Stop();
    }
    [Header("Animal Farm Sounds")]
    public AudioSource chickenSource;
    public AudioSource cowSource;
    public AudioSource sheepSource;
    public AudioSource duckSource;
    public AudioSource goatSource;

    public AudioClip[] chickenClips;
    public AudioClip[] cowClips;
    public AudioClip[] sheepClips;
    public AudioClip[] duckClips;
    public AudioClip[] goatClips;

    public float minDelay = 10f;
    public float maxDelay = 20f;


    IEnumerator AnimalLoop(AudioSource source, AudioClip[] clips)
    {
        while (true)
        {
            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);

            if (clips.Length == 0) continue;

            int index = Random.Range(0, clips.Length);

            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.8f, 1f);

            source.PlayOneShot(clips[index]);
        }
    }

    void DetectUIButton()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                Button btn = result.gameObject.GetComponent<Button>();

                if (btn != null)
                {
                    Debug.Log("Button Pressed: " + btn.name);
                    OnAnyButtonPressed(btn);
                    break;
                }
            }
        }
    }

    void OnAnyButtonPressed(Button button)
    {
        // Your global function
        Debug.Log("Detected Button From Anywhere: " + button.name);
        playClickSound();
    }
}
