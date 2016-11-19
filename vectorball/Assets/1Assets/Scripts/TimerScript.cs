using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public static TimerScript instance;

    float percent, secsPassed, countTillSecs;

    public event CallOnTimeOut TimeoutEvent;
    public delegate void CallOnTimeOut();

    public event TimerUpdate TimerUpdateEvent;
    public delegate void TimerUpdate(float percent);

    bool eventCalled = true;

	[SerializeField]
	private Image content;

    void Awake()
    {
        instance = this;
    }

	public void StartTimer(float seconds)
    {
		Debug.Log ("Inside StartTimer");
		this.countTillSecs = seconds;
        eventCalled = false;
    }

	private void HandleBar(){
		content.fillAmount = 1 - percent;
	}

    void Update()
    {
		HandleBar ();
		if(secsPassed < countTillSecs)
        {
            secsPassed += Time.deltaTime;

            if (TimeoutEvent != null)
            {
				Debug.Log (percent);
				percent = secsPassed / countTillSecs;
				TimerUpdateEvent(percent);
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