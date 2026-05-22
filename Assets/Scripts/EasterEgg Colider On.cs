using UnityEngine;
using UnityEngine.UI; // Utile si vous avez besoin d'importer des éléments d'UI spécifiques

public class EasterEggOn : MonoBehaviour
{
    [Tooltip("Glissez l'image (ou le GameObject de l'UI) ici depuis la hiérarchie")]
    public GameObject EasterEggImage;

    private void Start()
    {
        // On s'assure que l'image est cachée au début du jeu
        if (EasterEggImage != null)
        {
            EasterEggImage.SetActive(false);
        }
    }

    // Cette fonction se déclenche quand un autre objet entre dans le Trigger (zone de collision) de l'empty
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet qui touche l'empty est le joueur (le joueur doit avoir le tag "Player")
        if (other.CompareTag("Player"))
        {
            if (EasterEggImage != null)
            {
                EasterEggImage.SetActive(true); // Fait apparaître l'image
                AudioManager am = AudioManager.instance;
                am.PlaySFX(am.sfx_list.sfx_EasterEgg);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // On vérifie si l'objet qui touche l'empty est le joueur (le joueur doit avoir le tag "Player")
        if (other.CompareTag("Player"))
        {
            if (EasterEggImage != null)
            {
                EasterEggImage.SetActive(false); // Fait disparaître l'image
                //AudioManager am = AudioManager.instance;
                //am.PlaySFX(am.sfx_list.sfx_EasterEgg);
            }
        }
    }
}