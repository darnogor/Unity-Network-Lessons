using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class SetupLocalPlayer : NetworkBehaviour
{
	public Text namePrefab;
	public Text nameLabel;
	public Transform namePosition;
	private string textFieldName = "";
	private string textFieldColor = "";
	
	[SyncVar (hook = "OnNameChanged")]
	public string playerName = "player";
	
	[SyncVar (hook = "OnColorChanged")]
	public string playerColor = "#ffffff";


	public void UpdateStates()
	{
		OnNameChanged(playerName);
		OnColorChanged(playerColor);
	}

	public void OnNameChanged(string name)
	{
		playerName = name;
		nameLabel.text = playerName;
	}
	
	public void OnColorChanged(string color)
	{
		playerColor = color;

		var renderers = GetComponentsInChildren<Renderer>();
		foreach (var r in renderers)
		{
			if (r.gameObject.name == "BODY")
			{
				r.material.SetColor("_Color", ColorFromHex(playerColor));
			}
		}
	}

	[Command]
	public void CmdChangeName(string name)
	{
		playerName = name;
	}
	
	
	[Command]
	public void CmdChangeColor(string color)
	{
		playerColor = color;
	}
	

	private void OnGUI()
	{
		if (isLocalPlayer)
		{
			textFieldName = GUI.TextField(new Rect(25, 15, 100, 25), textFieldName);
			if (GUI.Button(new Rect(130, 15, 35, 25), "Set"))
			{
				CmdChangeName(textFieldName);
			}

			textFieldColor = GUI.TextField(new Rect(170, 15, 100, 25), textFieldColor);
			if (GUI.Button(new Rect(275, 15, 35, 25), "Set"))	
			{
				CmdChangeColor(textFieldColor);
			}
		}
	}


	private Color ColorFromHex(string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		byte a = 255;
		
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
		}
		
		return new Color32(r, g, b, a);
	}


	public override void OnStartClient()
	{
		base.OnStartClient();
		Invoke("UpdateStates", 1);
	}
	

	private void Start ()
	{
		GetComponent<PlayerController>().enabled = isLocalPlayer;
		if(isLocalPlayer)
		{
			CameraFollow360.player = this.gameObject.transform;
		}

		var canvas = GameObject.FindWithTag("MainCanvas");
		nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity);
		nameLabel.transform.SetParent(canvas.transform);
	}

	void Update()
	{
		if (Camera.main != null)
		{
			Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
			bool isOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && 
			                screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

			if (isOnScreen)
			{
				var namePos = Camera.main.WorldToScreenPoint(namePosition.position);
				nameLabel.transform.position = namePos;
			}
			else
			{
				nameLabel.transform.position = new Vector3(-1000,-1000,0);
			}
		}
	}


	private void OnDestroy()
	{
		if (nameLabel != null)
		{
			Destroy(nameLabel.gameObject);
		}
	}
}
