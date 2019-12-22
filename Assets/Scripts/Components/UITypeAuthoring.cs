using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class UITypeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public UIType.Type Value;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddSharedComponentData(entity, new UIType{ Value = Value });        
    }
}

[System.Serializable]
public struct UIType : ISharedComponentData
{
    public Type Value;

    public enum Type{
        SCORE = 0,
        Timer = 1
    }
}