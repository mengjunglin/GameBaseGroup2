using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;
    public Animator anim;

    void Awake()
    {
        isOpponent = (name.Contains("PositionB"));
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            PlayKickAnimation();
    }

    public void PlayKickAnimation()
    {
        anim.SetTrigger("Kick");
    }
}
