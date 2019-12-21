using System.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Scenes;

// ゲームの進行管理を行う
public class GameManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;
    [SerializeField] SubScene subScene;

    Entity controlable, gamestate;

    GameState state;


    IEnumerator Start()
    {
        state = new GameState();

        while (true)
        {
            yield return InitGame();

            yield return GamePlay();

            yield return GameSet();

        }
    }

    IEnumerator InitGame()
    {
        var loadParams = new SceneSystem.LoadParameters {};
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var sceneSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSystem>();
        var sceneEntity = sceneSystem.LoadSceneAsync(subScene.SceneGUID, loadParams);

        sceneSystem.EntityManager.AddComponentObject(sceneEntity, this);
        gamestate = manager.CreateEntity(typeof(GameState));
        label.enabled = true;

        for (int i = 3; i > 0; i--)
        {
            label.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        controlable = manager.CreateEntity(typeof(Controlable));
        label.text = "GO";

        yield return new WaitForSeconds(1);
        label.enabled = false;

    }

    IEnumerator GamePlay()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        return new WaitWhile((() =>
        {
            state = manager.GetComponentData<GameState>(gamestate);
            return (state.ItemCount != 0 && state.timer > 0);
        }));
    }

    IEnumerator GameSet()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        label.enabled = true;

        if (state.timer > 0)
            label.text = "WIN";
        else
            label.text = "LOSE";

        manager.DestroyEntity(controlable);

        yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.Space));

        var sceneSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SceneSystem>();
        sceneSystem.UnloadScene(subScene.SceneGUID, SceneSystem.UnloadParameters.DestroySceneProxyEntity | SceneSystem.UnloadParameters.DestroySectionProxyEntities);
    }

}