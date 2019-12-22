using System.Collections;
using Unity.Entities;
using UnityEngine;

//ゲームの進行管理を行う
public class GameManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;

    public float timer {get; set;}
    public int ItemCount{get; set;}

    IEnumerator Start()
    {
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        label.enabled = true;

        for(int i=3; i>0; i--)
        {
            label.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        var controlable = manager.CreateEntity(typeof(Controlable));
        label.text = "GO";

        yield return new WaitForSeconds(1);
        label.enabled = false;

        yield return new WaitWhile((()=>{
            return (ItemCount != 0 && timer > 0);
        }));

        manager.DestroyEntity(controlable);

        label.enabled = true;
        if( timer > 0)
            label.text = "WIN";
        else
            label.text = "LOSE";
    }
}