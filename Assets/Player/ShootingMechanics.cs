using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMechanics : MonoBehaviour
{
    
    public Animator animator;
    private bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isShooting = true;
            animator.SetBool("isShooting", true);
        } else
        {
            isShooting = false;
            animator.SetBool("isShooting", false);
        }
    }
}
