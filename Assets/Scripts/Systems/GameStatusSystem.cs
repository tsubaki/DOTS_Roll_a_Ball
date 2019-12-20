using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStatusSystem : JobComponentSystem
{
    EntityQuery query;

    protected override void OnCreate()
    {
        query = GetEntityQuery(typeof(Cube));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var count = query.CalculateEntityCount();

        Entities.ForEach((ref GameState state)=> state.ItemCount = count).Run();

        return inputDeps;
    }
}