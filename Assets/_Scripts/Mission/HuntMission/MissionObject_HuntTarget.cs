using System;
using UnityEngine;
using UnityEngine.Rendering;

public class MissionObject_HuntTarget : MonoBehaviour
{
    public static event Action OnTargetKilled;


    public void InvokeOnTargetKilled() => OnTargetKilled?.Invoke();


    
}
