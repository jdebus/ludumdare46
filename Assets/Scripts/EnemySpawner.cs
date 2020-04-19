using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject HunterPrefab;


    [SerializeField]
    float radius = 10;

    [SerializeField]
    float interval = 5;


    float timer = 0;

    bool started = false;

    bool isNight = false;

    private void Start()
    {
        GameManager.Instance.DayNight.OnNightStarted += () => isNight = true;
        GameManager.Instance.DayNight.OnDayStarted += (d) => isNight = false;
       // GameManager.Instance.DayNight.MidDay += DayNight_MidDay;

        GameManager.Instance.OnWin += () => enabled = false;
        GameManager.Instance.OnGameOver += () => enabled = false;
    }

    //private void DayNight_MidDay(int day)
    //{
    //    if (day == 1)
    //        started = true;
    //}

    void Update()
    {
        if (isNight)
            return;

        timer += Time.deltaTime;
        if(timer >= interval)
        {
            Spawn();
            timer = 0;
        }

    }

    void Spawn()
    {
        Vector2 pos = ((Vector2)Random.onUnitSphere).normalized * radius + (Vector2)transform.position;
        Instantiate(HunterPrefab, pos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

    }
}
