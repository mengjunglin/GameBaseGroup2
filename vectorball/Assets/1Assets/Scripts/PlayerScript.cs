using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;
    public Animator anim;
    public Transform ball;
	private int multiplier=0;

    [SerializeField]
    PlayFaceAnimations faceAnimController;

    void Awake()
    {
        isOpponent = (name.Contains("PositionB"));
        anim = GetComponentInChildren<Animator>();
        faceAnimController = GetComponentInChildren<PlayFaceAnimations>();
    }

    void Update()
    {
        if (ball)
        {
            Vector3 target = new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z);
            transform.LookAt(target);
        }
    }

    public void KickBallTowards(int x, int y)
    {
        PlayerScript target = FieldController.instance.GetPlayerAt(x, y);
        ball.GetComponent<BallMoveBehavior>().setTarget(target.transform);
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
        Invoke("ResetExpression", 2f);
    }

    public void ResetExpression()
    {
        faceAnimController.Eye = Eyes_Expressions.Happy;
    }
}
