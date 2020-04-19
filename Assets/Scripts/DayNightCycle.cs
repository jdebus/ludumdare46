using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    Light2D globalLight;

    [SerializeField]
    Light2D playerLight;

    [SerializeField]
    int dayDurationInSeconds = 120;

    [SerializeField]
    int nightStartAt = 80;

    public int Days { get; private set; } = 1;

    public event Action<int> OnDayStarted;
    public event Action OnNightStarted;
    public event Action<int> MidDay;

    float timer = 0;
    bool nightActive = false;
    bool midDayInvoked = false;

    public bool IsNight => nightActive;

    private void Start()
    {
        OnDayStarted?.Invoke(1);
        GameManager.Instance.OnWin += () => enabled = false;
        GameManager.Instance.OnGameOver += () => enabled = false;
        playerLight.enabled = false;
        globalLight.intensity = 1;
        playerLight.intensity = 1;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > nightStartAt && !nightActive)
        {
            OnNightStarted?.Invoke();
            nightActive = true;
            StartCoroutine(StartNight());
        }

        if (timer > nightStartAt * 0.4f && !midDayInvoked)
        {
            MidDay?.Invoke(Days);
            midDayInvoked = true;
        }

        if(timer > dayDurationInSeconds)
        {
            nightActive = false;
            timer = 0;
            midDayInvoked = false;
            StartCoroutine(StartDay());
            Days++;
            Debug.Log($"Day {Days} started");
            OnDayStarted?.Invoke(Days);
        }
    }


    IEnumerator StartNight()
    {
      
        float targetInensity = 0.2f;
        float duration = 4;
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            float p = time / duration;

            float i = Mathf.Lerp(0, 1-targetInensity, p);
            globalLight.intensity = 1 - i;
            //playerLight.intensity = Mathf.Lerp(0, 1, p);

            playerLight.enabled = p > 0.5f;

            yield return new WaitForEndOfFrame();
        }

        playerLight.enabled = true;
        globalLight.intensity = targetInensity;
    }

    IEnumerator StartDay()
    {
        globalLight.intensity = 0.2f;

        float duration = 4;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float p = time / duration;

            globalLight.intensity = Mathf.Lerp(0.2f, 1, p);
            //playerLight.intensity = 1 - Mathf.Lerp(0, 1, p);

            playerLight.enabled = p < 0.5f;

            yield return new WaitForEndOfFrame();
        }

        playerLight.enabled = false;
        globalLight.intensity = 1;

    }


}
