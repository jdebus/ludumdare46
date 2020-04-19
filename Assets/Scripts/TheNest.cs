using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheNest : MonoBehaviour
{
    public NestState State { get; set; } = NestState.EggInside;

    [SerializeField]
    TheEgg egg;
    public TheEgg Egg => egg;

    [SerializeField]
    TheBaby baby;
    public TheBaby Baby => baby;

    public BabyGrownUp babyGrownUp;

    public Cinemachine.CinemachineVirtualCamera babyCam;

    private void Start()
    {
        GameManager.Instance.DayNight.MidDay += MidDay;
        GameManager.Instance.OnWin += Instance_OnWin;
        State = NestState.EggInside;
        baby.gameObject.SetActive(false);
    }

    private void Instance_OnWin()
    {
        babyCam.Priority = 100;
        babyGrownUp.gameObject.SetActive(true);
        babyGrownUp.Move();
        baby.gameObject.SetActive(false);
    }

    private void MidDay(int day)
    {
        if(day == 2)
        {
            GameManager.Instance.TemporaryPaused = true;
            babyCam.Priority = 100;
            egg.Break();
        }
            
    }

    public void ActivateBaby()
    {
        if(State == NestState.EggInside)
        {
            State = NestState.BabyInside;
            egg.gameObject.SetActive(false);
            baby.gameObject.SetActive(true);
            baby.Activate();
        }
    }

    public void TakeDamage(int damage)
    {
        if(State == NestState.EggInside)
        {
            egg.TakeDamage(damage);
        }
        else if(State == NestState.BabyInside)
        {
            baby.TakeDamage(damage);
        }
    }

    public void OnMouseDown()
    {
        if (State == NestState.BabyInside)
            baby.MouseDown();

      
    }

}

public enum NestState
{
    EggInside,
    Empty,
    BabyInside
}
