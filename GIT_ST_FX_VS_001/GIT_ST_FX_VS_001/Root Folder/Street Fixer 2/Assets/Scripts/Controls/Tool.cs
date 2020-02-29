using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Tool : Item
{
    public PartType repairType;

    public AudioClip repairSound;
    public AudioClip damageSound;
}
