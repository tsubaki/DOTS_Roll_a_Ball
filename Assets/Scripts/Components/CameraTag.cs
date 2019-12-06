using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct CameraTag : IComponentData{

    public float3 offset;

}
