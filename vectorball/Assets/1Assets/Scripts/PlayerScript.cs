using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;
    public Animator anim;
    public Transform ball;

    void Awake()
    {
        isOpponent = (name.Contains("PositionB"));
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            PlayKickAnimation();

        if(ball)
            transform.LookAt(ball);
    }

    public void PlayKickAnimation()
    {
        anim.SetTrigger("Kick");
    }
}
