using Unity.Entities;
using Unity.Jobs;
using static Unity.Entities.ComponentType;

/// GameManagerが進行の管理を把握するための情報を収集する
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStatusSystem : JobComponentSystem
{
    private EntityQuery query, timerQuery;

    protected override void OnCreate()
    {
        query = GetEntityQuery(ReadOnly<Cube>());
        timerQuery = GetEntityQuery(ReadOnly<Timer>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var count = query.CalculateEntityCount();
        var timerValue = 10f;

        if( timerQuery.CalculateEntityCount() > 0 )
            using( var array = timerQuery.ToComponentDataArray<Timer>(Unity.Collections.Allocator.TempJob) )
                timerValue = array[0].Value;

        Entities
            .WithoutBurst()
            .ForEach((GameManager manager) =>
            {
                manager.ItemCount = count;
                manager.timer = timerValue;
            }).Run();

        return inputDeps;
    }
}