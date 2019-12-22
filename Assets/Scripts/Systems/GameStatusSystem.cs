using Unity.Entities;
using Unity.Jobs;
using static Unity.Entities.ComponentType;

/// GameManagerが進行の管理を把握するための情報を収集する
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStatusSystem : JobComponentSystem
{
    EntityQuery query, timerQuery;

    protected override void OnCreate()
    {
        query = GetEntityQuery(typeof(Cube));
        timerQuery = GetEntityQuery( ReadOnly<Timer>(), ReadOnly<GameStateTag>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var count = query.CalculateEntityCount();
        var timer = timerQuery.GetSingleton<Timer>().Value;
        Entities
            .WithoutBurst()
            .ForEach((GameManager manager)=>{
                manager.ItemCount = count;
                manager.timer = timer;
            }).Run();

        return inputDeps;
    }
}