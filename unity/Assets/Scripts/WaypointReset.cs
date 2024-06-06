using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointReset : MonoBehaviour
{
    public CharacterShirtController shirtController;
    public CharacterHairController hairController;
    public CharacterRandomOffset animatorRandomOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset() {
        transform.name = transform.name + "x";
        shirtController.reset();
        hairController.reset();
        animatorRandomOffset.reset();
    }
}
