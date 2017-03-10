using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayName : MonoBehaviour {

	public Text title;


	void OnMouseEnter()
	{
		switch(name)
		{
		case "Pac-Man - Catch Me":
			title.color = Color.yellow;
			break;

		case "Blinky - Shadow":
			title.color = Color.red;
			break;

		case "Inky - Bashful":
			title.color = Color.cyan;
			break;

		case "Clyde - Pokey":
			title.color = new Color(254f/255f, 203f/255f, 51f/255f);
			break;

        case "Pinky - Speedy":
            title.color = new Color(254f / 255f, 152f / 255f, 203f / 255f);
             break;
        }
		
		title.text = name;
	}

	void OnMouseExit()
	{
		title.text = "Pac-Man";
		title.color = Color.yellow;
	}
}
