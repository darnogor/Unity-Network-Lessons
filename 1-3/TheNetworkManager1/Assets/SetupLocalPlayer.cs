using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

	public override void OnStartServer()
	{
		Debug.Log("Starting Server"  + isLocalPlayer);
	}

	public override void OnStartClient()
	{
		Debug.Log("Starting Client"  + isLocalPlayer);
	}

	private void Awake()
	{
		Debug.Log("Awaking Player " + isLocalPlayer);
	}

	// Use this for initialization
	private void Start ()
	{
		GetComponent<PlayerController>().enabled = isLocalPlayer;
		Debug.Log("Starting Player " + isLocalPlayer);
	}


}
