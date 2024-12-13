using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissoesController : MonoBehaviour
{
    [SerializeField] Mission[] missions;
    bool allMissionsDelivered;


    public void FimDeJogo()
    {
        if (AllMissionsCompleted())
        {
            CanvasController.Singleton.OpenEndGame();
        }
    }
    private bool AllMissionsCompleted()
    {
        for (int i = 0; i < missions.Length; i++  )
        {
            if (missions[i].isMissionCompleted == false )
            {
                return false;
            }
        }
        return true;
    }
}
