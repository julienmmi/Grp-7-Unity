using UnityEngine;
using System.Collections.Generic;

public class VerrouPorte : MonoBehaviour
{
    
    public List<Levier> leviers;
    private string validationVerrou = "Ok";

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
            //Ouvrir la porte
            //jouer un son
        }
    }
    }
}
