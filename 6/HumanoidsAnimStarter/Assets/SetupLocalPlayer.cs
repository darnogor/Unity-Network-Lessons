using UnityEngine;
using UnityEngine.Networking;


public class SetupLocalPlayer : NetworkBehaviour
{
    private Animator _animator;
    
    [SyncVar (hook = "OnChangeAnimation")]
    public string animState;


    void OnChangeAnimation(string state)
    {
        if (!isLocalPlayer)
        {
            UpdateAnimationState(state);
        }
    }
    
    [Command]
    public void CmdChangeAnimationState(string state)
    {
          UpdateAnimationState(state);  
    }


    private void UpdateAnimationState(string state)
    {
        if (animState == state)
            return;

        animState = state;
        Debug.Log(state);

        if (animState == "idle")
            _animator.SetBool("Idling", true);
        else if (animState == "run")
            _animator.SetBool("Idling", false);
        else if (animState == "attack")
            _animator.SetTrigger("Attacking");
    }
      

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.SetBool("Idling", true);

        GetComponent<PlayerController>().enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            CameraFollow360.player = this.gameObject.transform;
        }
    }
}