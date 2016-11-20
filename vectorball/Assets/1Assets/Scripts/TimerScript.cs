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

	void Start()
	{
		instance = this;
	}

    void Awake()
    {
        instance = this;
    }

    public void StartTimer(float seconds)
    {
		this.secsPassed = 0;
		this.countTillSecs = seconds;
        eventCalled = false;
        if(TimerUpdateEvent != null)
            TimerUpdateEvent(0);
    }

    public void StopTimer()
    {
        eventCalled = true;
        secsPassed = countTillSecs;
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