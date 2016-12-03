using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    [SerializeField] Button[] levelButtons;

    [SerializeField]
    GameObject creditsPopUp, character;

	// Use this for initialization
	void Start () {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedTillLevel", -1);

        for(int i=0;i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i < unlockedLevel -1);

        }

        CloseCreditsScreen();
	}
    
	public void ClickExit()
	{
		Application.Quit ();
	}
		
	public void OpenScene()
	{
		Application.LoadLevel (ChooseOptionsManagerScript.level);
	}
		

	//one method for each button, correspond to different levels
	public void OpenLevel1(int level)
	{
		ChooseOptionsManagerScript.level = level;
		Application.LoadLevel ("Intro"+level);
	}

	public void Deleteprefs()
	{
		PlayerPrefs.DeleteAll ();
		Application.Quit ();
	}

    public void OpenCreditsScreen()
    {
        creditsPopUp.SetActive(true);
        character.SetActive(false);
    }

    public void CloseCreditsScreen()
    {
        creditsPopUp.SetActive(false);
        character.SetActive(true);
    }

	public void OpenUnit2Screen(){
		Application.LoadLevel (3);
	}
}
