using UnityEngine;
using UnityEngine.UI; // Utile si vous avez besoin d'importer des éléments d'UI spécifiques

public class findujeu : MonoBehaviour
{
    [Tooltip("Glissez l'image (ou le GameObject de l'UI) ici depuis la hiérarchie")]
    public GameObject imageDeFin;

    private void Start()
    {
        // On s'assure que l'image est cachée au début du jeu
        if (imageDeFin != null)
        {
            imageDeFin.SetActive(false);
        }
    }

    // Cette fonction se déclenche quand un autre objet entre dans le Trigger (zone de collision) de l'empty
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet qui touche l'empty est le joueur (le joueur doit avoir le tag "Player")
        if (other.CompareTag("Player"))
        {
            if (imageDeFin != null)
            {
                imageDeFin.SetActive(true); // Fait apparaître l'image
            }
        }
    }

    // --- OPTIONNEL --- 
    // Si par "touché", vous vouliez dire "cliqué avec la souris", utilisez plutôt ceci :
    /*
    private void OnMouseDown()
    {
        if (imageDeFin != null)
        {
            imageDeFin.SetActive(true);
        }
    }
    */
}
