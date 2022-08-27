using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    private GhoulAI ghoulAI;
    private EnemyAnimation ea;

    void Awake()
    {
        if (ghoulAI == null) ghoulAI = GetComponent<GhoulAI>();
        if (ea == null) ea = GetComponent<EnemyAnimation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ghoulAI != null)
            ghoulAI.Movement();
    }
}
