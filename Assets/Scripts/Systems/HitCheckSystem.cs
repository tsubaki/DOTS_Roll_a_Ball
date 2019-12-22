using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// CubeとBallの接触判定を行う。接触時に `RequestScoreUp` を生成する。
// 接触したCubeは破棄される。
public class HitCheckSystem : JobComponentSystem
{
    private EntityCommandBufferSystem commandBufferSystem;
    private Entity socreupEntity;

    protected override void OnCreate()
    {
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        socreupEntity = EntityManager.CreateEntity(typeof(Prefab), typeof(RequestScoreUp));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var playerPos = EntityManager.GetComponentData<LocalToWorld>(GetSingletonEntity<Ball>()).Position;
        var commadnBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
        var prefabEntity = socreupEntity;

        inputDeps = Entities
            .WithAll<Cube>()
            .ForEach((Entity entity, int entityInQueryIndex, in LocalToWorld pos) =>
            {
                if (math.distance(playerPos, pos.Position) < 1)
                {
                    commadnBuffer.DestroyEntity(entityInQueryIndex, entity);
                    commadnBuffer.Instantiate(entityInQueryIndex, prefabEntity);
                }
            }).Schedule(inputDeps);

        commandBufferSystem.AddJobHandleForProducer(inputDeps);
        return inputDeps;
    }
}