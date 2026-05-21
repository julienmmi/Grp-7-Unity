using UnityEngine;
using System.Collections.Generic;

public class VerrouPorte : MonoBehaviour
{
    
    public List<Levier> leviers;
    private bool validationVerrou = true;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        validationVerrou = true;
        foreach (Levier levier in leviers)
        {
            if (levier.isActivated != levier.doitEtre)
            {
                validationVerrou = false;
                break;
            }
        }

        if (validationVerrou == true)
        {
            animator.SetBool("IsOpen", true);
            AudioManager am = AudioManager.instance;
            am.PlaySFX(am.sfx_list.sfx_victory);
        }
        
    }
}
