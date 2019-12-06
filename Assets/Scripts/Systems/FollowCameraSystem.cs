using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// 対象を追跡するカメラを制御する。
// offsetの数だけ離れた位置で追跡する。
public class FollowCameraSystem : JobComponentSystem {
    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var positions = new NativeArray<float3> (1, Allocator.TempJob);

        inputDeps = Entities
            .WithAll<FollowByCamera> ()
            .WithNativeDisableContainerSafetyRestriction (positions)
            .ForEach ((ref LocalToWorld local) => {
                positions[0] = local.Position;
            }).Schedule (inputDeps);

        inputDeps = Entities
            .ForEach ((ref Translation pos, in CameraTag camera) => {
                pos.Value = positions[0] + camera.offset;
            }).Schedule (inputDeps);

        positions.Dispose (inputDeps);

        return inputDeps;
    }
}