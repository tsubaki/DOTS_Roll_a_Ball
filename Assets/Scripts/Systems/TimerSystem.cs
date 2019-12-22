using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class TimerSystem : JobComponentSystem
{

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<Controlable>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var timerValue = new NativeArray<float>(1, Allocator.Temp);

        Entities
            .WithoutBurst()
            .ForEach((ref Timer timer) =>
            {
                timer.Value -= Time.DeltaTime;
                timerValue[0] = timer.Value;
            }).Run();

        Entities
            .WithoutBurst()
            .WithSharedComponentFilter(new UIType { Value = UIType.Type.Timer })
            .ForEach((TMPro.TextMeshPro label) =>
            {
                label.text = timerValue[0].ToString("000.00");
            }).Run();

        timerValue.Dispose();
        return inputDeps;
    }
}
