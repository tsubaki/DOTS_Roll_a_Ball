using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Force : IComponentData
{    
    [Range(0, 3)]
    public float magnitude;
}
