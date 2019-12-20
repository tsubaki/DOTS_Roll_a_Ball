using System.Collections;
using Unity.Entities;
using UnityEngine;

// ゲームの進行管理を行う
public class GameManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;

    Entity controlable, gamestate;

    IEnumerator Start()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        gamestate = manager.CreateEntity(typeof(GameState));
        label.enabled = true;

        for(int i=3; i>0; i--)
        {
            label.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        controlable = manager.CreateEntity(typeof(Controlable));
        label.text = "GO";

        yield return new WaitForSeconds(1);
        label.enabled = false;

        GameState state = new GameState();
        yield return new WaitWhile((()=>{
            state = manager.GetComponentData<GameState>(gamestate);
            return (state.ItemCount != 0 && state.timer > 0);
        }));

        label.enabled = true;

        if( state.timer > 0)
            label.text = "WIN";
        else
            label.text = "LOSE";
            
        manager.DestroyEntity(controlable);
    }
}