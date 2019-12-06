using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using TMPro;
using static Unity.Entities.ComponentType;

public class MoveBallSystem : JobComponentSystem {
    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var DeltaTime = Time.DeltaTime;
        var duration = new float3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        inputDeps = Entities.WithAll<Ball> ().ForEach ((ref PhysicsVelocity physicsVelocity, in PhysicsMass physicsMass, in Force force) => {
            physicsVelocity.Linear += (physicsMass.InverseMass * force.magnitude * DeltaTime) * duration;
        }).WithName ("Move").Schedule (inputDeps);

        return inputDeps;
    }
}

public class CubeRotationSystem : JobComponentSystem {
    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var axis = quaternion.RotateY (5 * Time.DeltaTime);
        inputDeps = Entities.WithAll<Cube> ().ForEach ((ref Rotation rot) => {
            rot.Value = math.mul (rot.Value, axis);
        }).WithName ("Rotation").Schedule (inputDeps);

        return inputDeps;
    }
}

public class HitCheckSystem : JobComponentSystem {
    private EntityCommandBufferSystem commandBufferSystem;
    private Entity socreupEntity;

    protected override void OnCreate () {
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem> ();
        socreupEntity = EntityManager.CreateEntity (typeof (Prefab), typeof (RequestScoreUp));
    }

    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var playerPos = EntityManager.GetComponentData<LocalToWorld> (GetSingletonEntity<Ball> ()).Position;
        var commadnBuffer = commandBufferSystem.CreateCommandBuffer ().ToConcurrent ();
        var prefabEntity = socreupEntity;

        inputDeps = Entities.WithAll<Cube> ().ForEach ((Entity entity, int entityInQueryIndex, in LocalToWorld pos) => {
            if (math.distance (playerPos, pos.Position) < 1) {
                commadnBuffer.DestroyEntity (entityInQueryIndex, entity);
                commadnBuffer.Instantiate (entityInQueryIndex, prefabEntity);
            }
        }).WithName ("HitCheck").Schedule (inputDeps);

        commandBufferSystem.AddJobHandleForProducer (inputDeps);
        return inputDeps;
    }
}

public class ScoreSystem : JobComponentSystem {
    EntityQuery scoreUpQuery;

    protected override void OnCreate () {
        scoreUpQuery = GetEntityQuery (ReadOnly<RequestScoreUp> ());
        RequireForUpdate (scoreUpQuery);
    }

    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var requestCount = scoreUpQuery.CalculateEntityCount ();
        TextMeshProUGUI g;

        Entities.WithoutBurst ().ForEach ((TextMeshProUGUI label, ref Score score) => {
            score.Value += requestCount;
            label.text = score.Value.ToString("00");
        }).Run ();

        EntityManager.DestroyEntity (scoreUpQuery);

        return inputDeps;
    }
}