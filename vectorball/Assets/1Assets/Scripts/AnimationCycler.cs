using UnityEngine;
using System.Collections;

/// <summary>
/// Quick and dirty implementation
/// </summary>
public class AnimationCycler : MonoBehaviour {


    PlayFaceAnimations pfScript;

    [SerializeField]
    Eyes_Expressions[] anims;

    [SerializeField]
    float[] duration;

    [SerializeField]
    int index;

	// Use this for initialization
	void Start () {
        pfScript = GetComponent<PlayFaceAnimations>();
        StartCoroutine(CycleAnims());
	}
	
	IEnumerator CycleAnims()
    {
        while(true)
        {
            yield return new WaitForSeconds(duration[index]);

            index++;
            index %= anims.Length;

            pfScript.Eye = anims[index];
        }
    }

}
