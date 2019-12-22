using System.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Scenes;

//ゲームの進行管理を行う
public class GameManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;
    [SerializeField] SubScene subScene;

    public float timer {get; set;}
    public int ItemCount{get; set;}

    Entity controlable;

    IEnumerator Start()
    {
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
            return (ItemCount != 0 && timer > 0);
        }));
    }

    IEnumerator GameSet()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        label.enabled = true;

        if (timer > 0)
            label.text = "WIN";
        else
            label.text = "LOSE";
            
        manager.DestroyEntity(controlable);

        yield return new WaitUntil(()=> Input.GetKeyDown(KeyCode.Space));

        var sceneSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SceneSystem>();
        sceneSystem.UnloadScene(subScene.SceneGUID, SceneSystem.UnloadParameters.DestroySceneProxyEntity | SceneSystem.UnloadParameters.DestroySectionProxyEntities);
    }
}