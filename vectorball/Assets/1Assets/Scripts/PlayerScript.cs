using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public bool isOpponent;

    void Awake()
    {
        isOpponent = (name.Contains("PositionB"));
    }

    public void PlayKickAnimation()
    {

    }
}
