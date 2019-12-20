using Unity.Entities;
using Unity.Jobs;

/// GameManagerが進行の管理を把握するための情報を収集する
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
        inputDeps = Entities.ForEach((ref GameState state)=> state.ItemCount = count).Schedule(inputDeps);

        return inputDeps;
    }
}