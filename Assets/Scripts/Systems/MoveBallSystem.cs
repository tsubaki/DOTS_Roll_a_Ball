using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

// InputManagerのVertical とHorizontal の情報に合わせてボールを動かす。
// ボールはUnity.Physics上で動作。
// カメラの向きは考慮しない。
public class MoveBallSystem : JobComponentSystem {
    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var DeltaTime = Time.DeltaTime;
        var duration = new float3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        inputDeps = Entities
            .WithAll<Ball> ()
            .WithName ("Move")
            .ForEach ((ref PhysicsVelocity physicsVelocity, in PhysicsMass physicsMass, in Force force) => {
                physicsVelocity.Linear += (physicsMass.InverseMass * force.magnitude * DeltaTime) * duration;
            }).Schedule (inputDeps);

        return inputDeps;
    }
}