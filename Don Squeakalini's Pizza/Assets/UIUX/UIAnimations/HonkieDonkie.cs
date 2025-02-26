using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkieDonkie : MonoBehaviour
{
    public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Do ()
    {
        _animator.SetBool("New Bool", !_animator.GetBool("New Bool"));
    }
}
