using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

// Cubeを回転させるだけのシステム
// read : Cube, Rotation
// write : Rotation
public class CubeRotationSystem : JobComponentSystem {
    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var axis = quaternion.RotateY (5 * Time.DeltaTime);
        inputDeps = Entities
            .WithAll<Cube> ()
            .WithName ("Rotation")
            .ForEach ((ref Rotation rot) => {
                rot.Value = math.mul (rot.Value, axis);
            }).Schedule (inputDeps);

        return inputDeps;
    }
}