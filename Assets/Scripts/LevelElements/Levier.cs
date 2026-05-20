using UnityEngine;

public class Levier : MonoBehaviour
{
    public bool isActivated = false;
    private bool playerContact = false;
    public bool doitEtre = false;
    public Material material1;
    public Material material2;
    private MeshRenderer meshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update left intentionally empty. Use OnClick() to toggle the lever.
        if (playerContact) {
			if(Input.GetKeyDown(KeyCode.F)) {
				isActivated = !isActivated;
                meshRenderer.material = material1;
                if (isActivated) {
                    meshRenderer.material = material2;
                }
			}
			
		}
    }

    //Si on touche son collider
	void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player" && !playerContact){
            playerContact = true;
        }
	}
	
	//Si on sort du collider
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player"){
			playerContact = false;
		}
	}
    
}
