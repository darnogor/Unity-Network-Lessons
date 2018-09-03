using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class SetupLocalPlayer : NetworkBehaviour {

	Animator animator;

	[SyncVar (hook = "OnChangeAnimation")]
	public string animState = "idle";

	[SyncVar (hook = "OnChangedTextureId")]
	public int textureId = 0;

	void OnChangeAnimation (string aS)
    {
		if(isLocalPlayer) return;
		UpdateAnimationState(aS);
    }


	[Command]
	public void CmdChangeAnimState(string aS)
	{
		UpdateAnimationState(aS);
	}

	void UpdateAnimationState(string aS)
	{
		if(animState == aS) return;
		animState = aS;
		if(animState == "idle")
			animator.SetBool("Idling",true);
		else if (animState == "run")
			animator.SetBool("Idling",false);
	}


	[Command]
	void CmdUpdatePlayerCharacter(int id)
	{
		NetworkManager.singleton.GetComponent<CustomNetworkManager>().SwitchPlayer(this, id);
	}


	[Command]
	public void CmdChangeTexture(int id)
	{
		this.changeTexture(id);
	}


	private void OnChangedTextureId(int id)
	{
		if (!isLocalPlayer)
		{
			this.changeTexture(id);
		}
	}


	private void changeTexture(int id)
	{
		textureId = id;
		this.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture =
			CharacterCustomizer.GetTop(id, this.name);
	}


	private void OnGUI()
	{
		if (isLocalPlayer)
		{
			if (Event.current.Equals(Event.KeyboardEvent("1"))
			    || Event.current.Equals(Event.KeyboardEvent("2"))
			    || Event.current.Equals(Event.KeyboardEvent("3"))
			    || Event.current.Equals(Event.KeyboardEvent("4")))
			{
				int id = int.Parse(Event.current.keyCode.ToString().Substring(5));
				CmdUpdatePlayerCharacter(id);
			}
		}
	}


	// Use this for initialization
	void Start () 
	{
		animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idling", true);
		
		GetComponent<PlayerController>().enabled = isLocalPlayer;
		this.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture =
			CharacterCustomizer.GetTop(textureId, this.name);

		if(isLocalPlayer)
		{
			CameraFollow360.player = this.gameObject.transform;
			CharacterCustomizer.character = this.gameObject;
			CmdChangeTexture(CharacterCustomizer.CurrentTextureId);
		}
	}
}
