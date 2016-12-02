using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] Button[] levelButtons;

	// Use this for initialization
	void Start () {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedTillLevel", -1);

        for(int i=0;i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i < unlockedLevel -1);
        }
	}
	

	public void ClickExit()
	{
		Application.Quit ();
	}
		
	public void OpenScene()
	{
		Application.LoadLevel (1);
	}

	//one method for each button, correspond to different levels
	public void OpenLevel1(int level)
	{
		ChooseOptionsManagerScript.level = level;
		Application.LoadLevel (1);
	}

	public void Deleteprefs()
	{
		PlayerPrefs.DeleteAll ();
		Application.Quit ();
	}
}
