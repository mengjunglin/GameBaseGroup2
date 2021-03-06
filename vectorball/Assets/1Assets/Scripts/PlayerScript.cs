﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;
    public Animator anim;
	private int multiplier=0;
    public TextMesh label;
    public SpriteRenderer arrowMarker;
	public Vector3 idlePosition, lastPosition, permPosition;

    bool currentlyCharging = false;

    PlayFaceAnimations faceAnimController;
    Character_Animations playerAnimator;
    float idleLerpCounter = 0;
    Vector3 lookAtTarget = Vector3.zero;

    public static Transform ball;
    public static float chargePercent = 0.6f;

    const float stopAtPercent = .92f;
    const float kickDelay = 2;
    const float animDelay = 1.7f;
    const float reverseSpeedFactor = 2;

    [SerializeField ]
    ChooseOptionsManagerScript chooseOptionsScript;
    [SerializeField]
    GameSceneScript gameSceneScript;

    void Awake()
    {
        isOpponent = (name.Contains("PositionB"));
        anim = GetComponentInChildren<Animator>();
        faceAnimController = GetComponentInChildren<PlayFaceAnimations>();
        playerAnimator = GetComponentInChildren<Character_Animations>();

        chooseOptionsScript = FindObjectOfType<ChooseOptionsManagerScript>();
        gameSceneScript = FindObjectOfType<GameSceneScript>();
    }

    void Update()
    {
        if(playerAnimator != null && playerAnimator.v != 0 && idleLerpCounter > kickDelay)
        {
            lookAtTarget = new Vector3(idlePosition.x, transform.position.y, idlePosition.z);
        }
        else if (ball)
        {
            lookAtTarget = new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z);            
        }

        if (transform.position != idlePosition)
        {
			if((idleLerpCounter > animDelay) && playerAnimator != null)
            {
                playerAnimator.v = 1;
                playerAnimator.run = 0.18f;
            }
            if (idleLerpCounter > kickDelay )
            {
                transform.position = Vector3.Lerp(lastPosition, idlePosition, (idleLerpCounter - kickDelay) / reverseSpeedFactor);
            }

            idleLerpCounter += Time.deltaTime;

            if((idleLerpCounter - kickDelay) / reverseSpeedFactor > stopAtPercent)
                playerAnimator.v = 0;
        }
        //else
        //    playerAnimator.v = 0;
		if(anim != null)
        anim.transform.LookAt(lookAtTarget);
    }

    public void ChargeTowardsBall(float percent, Vector3 targetPosition)
    {
        idleLerpCounter = 0;
        if (percent < stopAtPercent)
        {            
            transform.position = Vector3.Lerp(idlePosition, targetPosition + new Vector3(0,0,5.5f), (percent - chargePercent) * (1 / (1 - chargePercent)));
            //print(gameObject.name + " " + (percent - chargePercent) * (1 / (1 - chargePercent)) + targetPosition);
            lastPosition = transform.position;
            playerAnimator.v = 1;
            playerAnimator.run = 0.18f;
        }
        else
        {
            if (playerAnimator.v != 0)
            {
                BallMoveBehavior bmb = ball.GetComponent<BallMoveBehavior>();
                bmb.target = transform;
                PlayerScript ps = null;

                while (ps == null || ps == this)
                {
                    ps = FieldController.instance.GetRandomOpponent();
                }

                Vector2 coordinates = FieldController.instance.GetXYOfPlayer(ps);

                if (chooseOptionsScript.tries < 1)
                {
                    KickBallTowards(Mathf.RoundToInt(coordinates.x), Mathf.RoundToInt(coordinates.y));
                    print("pass to:" + coordinates);
                }
                else
                {
                    PassToLastPlayerAndScore(true);
                    print("pass to LAST player");
                }
            }
            playerAnimator.v = 0;
            playerAnimator.run = 0;
        }
    }

    public void KickBallTowards(int x, int y)
    {
        PlayerScript target = FieldController.instance.GetPlayerAt(x, y);
        BallMoveBehavior ballMono = ball.GetComponent<BallMoveBehavior>();
        ballMono.setTarget(target.transform);
    }

    public void PlayKickAnimation()
    {
        anim.SetTrigger("Kick");
    }

	public void setMultiplier(int m){
		multiplier = m;
	}

	public int getMultiplier(){
		return multiplier ;
	}

    public void PlayExpression(bool happy)
    {
        faceAnimController.Eye = happy ? Eyes_Expressions.Closed_smile : Eyes_Expressions.Sad;
        Invoke("ResetPlayer", 2f);
    }

    public void ResetPlayer()
    {
        faceAnimController.Eye = Eyes_Expressions.Happy;
        if(label)
            label.color = Color.black;
        arrowMarker.gameObject.SetActive(false);
    }

    public void HighlightPlayer()
    {
        if (label)
            label.color = Color.green;
        arrowMarker.gameObject.SetActive(true);
        faceAnimController.Eye = Eyes_Expressions.Closed_happy;
        //Invoke("ResetPlayer", 2f);
    }

    /// <summary>
    /// Only called to score own teams goal
    /// </summary>
    public void PassToLastPlayerAndScore(bool opponent)
    {
        int yAxis = (opponent ? 1 : 5);

        KickBallTowards(0, yAxis);
        PlayerScript target = FieldController.instance.GetPlayerAt(0, yAxis);
        print(target + " target" + name);
        target.StartCoroutine("KickToGoal");
    }

    IEnumerator KickToGoal()
    {
        int delay = 0;
        int waitFor = 5;
        while(delay < waitFor)
        {
            yield return new WaitForSeconds(1);
            delay += 1;
            print("Kicking to " + isOpponent + name + " goal in:" + (waitFor - delay));
        }

        int index;

        index = (isOpponent ? 0 : 1);

        Vector3 offset = new Vector3(0, Random.Range(10f,35f) * (Random.Range(0,2) == 0 ? -1 : 1), Random.Range(0, 30f));

        BallMoveBehavior ballMono = ball.GetComponent<BallMoveBehavior>();
        ballMono.setTarget(FieldController.instance.goalsTransform[index]);

        TimerScript.instance.StopTimer();

        gameSceneScript.OnGoalScored(!isOpponent);
    }
}
