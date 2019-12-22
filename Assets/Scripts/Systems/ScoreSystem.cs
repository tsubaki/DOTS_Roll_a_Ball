using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using static Unity.Entities.ComponentType;

// RequestScoreUp に反応してスコアを上昇させ、UIに反映させる。
// RequestScoreUp があるときのみ動作
public class ScoreSystem : JobComponentSystem
{
    private EntityQuery scoreUpQuery;

    protected override void OnCreate()
    {
        scoreUpQuery = GetEntityQuery(ReadOnly<RequestScoreUp>());
        RequireForUpdate(scoreUpQuery);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var requestCount = scoreUpQuery.CalculateEntityCount();
        var currentScore = new NativeArray<int>(1, Allocator.Temp);

        Entities
            .ForEach((ref Score score) =>
            {
                score.Value += requestCount;
                currentScore[0] = score.Value;
            }).Run();

        Entities
            .WithoutBurst()
            .WithSharedComponentFilter(new UIType { Value = UIType.Type.SCORE })
            .ForEach((TextMeshProUGUI label) =>
            {
                label.text = currentScore[0].ToString("00");
            }).Run();

        EntityManager.DestroyEntity(scoreUpQuery);
        currentScore.Dispose();

        return inputDeps;
    }
}