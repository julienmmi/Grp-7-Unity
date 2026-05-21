using UnityEngine;
using System.Collections.Generic;

public class VerrouPorte : MonoBehaviour
{
    
    public List<Levier> leviers;
    private string validationVerrou = "Ok";
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Levier levier in leviers) {
        if (levier.isActivated == levier.doitEtre) {
            validationVerrou = "Ok";
        } else {
            validationVerrou = "Pas Ok";
            break;
        }
        if (validationVerrou == "Ok") {
            animator.SetBool("IsOpen", true);
            AudioManager am = AudioManager.instance;
			am.PlaySFX(am.sfx_list.sfx_victory);
        }
    }
    }
}
