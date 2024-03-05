using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyCollision : MonoBehaviour
{
    public Material materialDamaged;
    public Material materialNormal;

    private MeshRenderer mr;

    private void OnTriggerEnter(Collider other)
    {
        mr.material = materialDamaged;

        DOVirtual.DelayedCall(0.1f, () =>
        {
            mr.material = materialNormal;
        });
        
    }
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
