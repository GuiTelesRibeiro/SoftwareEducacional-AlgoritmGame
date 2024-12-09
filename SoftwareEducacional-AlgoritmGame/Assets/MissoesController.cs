using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissoesController : MonoBehaviour
{
    [SerializeField] Mission[] missions;
    bool allMissionsDelivered;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FimDeJogo()
    {
        if (AllMissionsCompleted())
        {
            Debug.Log("Fim de jogo");
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
