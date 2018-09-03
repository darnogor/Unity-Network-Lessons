using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    private static CharacterCustomizer CC;
    public static GameObject character;
    public Texture[] JaneTops;
    public Texture[] BrutiusTops;

    private int _currentTextureId;

    public void ChangeTopTexture(int i)
    {
        var topChild = character.transform.Find("Tops");
        if (character.name.Contains("Jane"))
            topChild.GetComponent<Renderer>().material.mainTexture = JaneTops[i];
        else if (character.name.Contains("Brutius"))
            topChild.GetComponent<Renderer>().material.mainTexture = BrutiusTops[i];

        character.GetComponent<SetupLocalPlayer>().CmdChangeTexture(i);
        _currentTextureId = i;
    }


    public static Texture GetTop(int i, string name)
    {
        if (name.Contains("Jane"))
            return CC.JaneTops[i];
        if (name.Contains("Brutius"))
            return CC.BrutiusTops[i];

        return null;
    }


    public static int CurrentTextureId
    {
        get { return CC._currentTextureId; }
    }


    void Start()
    {
        CC = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}