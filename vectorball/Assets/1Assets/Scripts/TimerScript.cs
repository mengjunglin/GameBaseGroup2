using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {

    public static TimerScript instance;

    float percent, secsPassed, countTillSecs;

    public event CallOnTimeOut TimeoutEvent;
    public delegate void CallOnTimeOut();

    public event TimerUpdate TimerUpdateEvent;
    public delegate void TimerUpdate(float percent);

    bool eventCalled = true;

    void Awake()
    {
        instance = this;
    }

    public void StartTimer(float seconds, float percent )
    {
        this.percent = percent;
        this.countTillSecs = seconds;
        eventCalled = false;
    }

    void Update()
    {
        if(secsPassed < countTillSecs)
        {
            secsPassed += Time.deltaTime;

            if (TimeoutEvent != null)
            {
                TimerUpdateEvent(secsPassed / countTillSecs);
            }
        }
        else
        {
            if (!eventCalled)
            {
                if (TimeoutEvent != null)
                {
                    TimerUpdateEvent(1);
                }
                if (TimeoutEvent != null)
                {
                    TimeoutEvent();
                    //foreach (CallOnTimeOut d in TimeoutEvent.GetInvocationList())
                    //{
                    //    TimeoutEvent -= (CallOnTimeOut) d;
                    //}
                }
                eventCalled = true;
            }
        }     
    }
}