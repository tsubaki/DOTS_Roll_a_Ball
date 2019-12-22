using Unity.Entities;
using Unity.Jobs;
using static Unity.Entities.ComponentType;

/// GameManagerが進行の管理を把握するための情報を収集する
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStatusSystem : JobComponentSystem
{
    EntityQuery query;

    protected override void OnCreate()
    {
        query = GetEntityQuery(ReadOnly<Cube>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var count = query.CalculateEntityCount();
        var timerValue = 10f;

        Entities
            .WithoutBurst()
            .ForEach((ref Timer timer)=>{
                timerValue = timer.Value;
            }).Run();

        Entities
            .WithoutBurst()
            .ForEach((GameManager manager)=>{
                manager.ItemCount = count;
                manager.timer = timerValue;
            }).Run();

        return inputDeps;
    }
}