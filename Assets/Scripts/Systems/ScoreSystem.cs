using TMPro;
using Unity.Entities;
using Unity.Jobs;
using static Unity.Entities.ComponentType;

// RequestScoreUp に反応してスコアを上昇させ、UIに反映させる。
// RequestScoreUp があるときのみ動作
public class ScoreSystem : JobComponentSystem {
    EntityQuery scoreUpQuery;

    protected override void OnCreate () {
        scoreUpQuery = GetEntityQuery (ReadOnly<RequestScoreUp> ());
        RequireForUpdate (scoreUpQuery);
    }

    protected override JobHandle OnUpdate (JobHandle inputDeps) {
        var requestCount = scoreUpQuery.CalculateEntityCount ();

        Entities
            .ForEach((ref Score score)=>{
                score.Value += requestCount;
            }).Run();

        Entities
            .WithoutBurst ()
            .ForEach ((TextMeshProUGUI label, in Score score) => {
                label.text = score.Value.ToString ("00");
            }).Run ();

        EntityManager.DestroyEntity (scoreUpQuery);

        return inputDeps;
    }
}