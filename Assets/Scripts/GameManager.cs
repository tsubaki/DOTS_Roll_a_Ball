using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI label;

    IEnumerator Start()
    {
        for(int i=3; i>0; i--)
        {
            label.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        label.enabled = false;

        World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(Controlable));
    }
}
