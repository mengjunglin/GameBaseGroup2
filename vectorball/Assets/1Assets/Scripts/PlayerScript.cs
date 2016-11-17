using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;
    public Animator anim;
    public Transform ball;
	private int multiplier=0;
    public TextMesh label;
    public SpriteRenderer arrowMarker;

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
            anim.transform.LookAt(target);
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
}
