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
        Entities
            .WithoutBurst()
            .ForEach((ref Timer timer)=>{
                timer.Value -= Time.DeltaTime;
            }).Run();

        Entities
            .WithoutBurst()
            .ForEach((TMPro.TextMeshPro label, ref Timer timer)=>{
                label.text = timer.Value.ToString("000.00");
            }).Run();


        return inputDeps;
    }
}
