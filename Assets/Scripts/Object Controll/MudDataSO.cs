using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MudData", menuName = "Game Data/Mud Data")]
public class MudDataSO : ScriptableObject
{
    public GameObject[] mudObjects;
    public GameObject[] mudChildren;
}
