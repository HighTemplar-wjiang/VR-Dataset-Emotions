using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShirtController : MonoBehaviour
{
    public Material[] materials;
    public Material selectedColor;
    public string shirtcolor;
    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSelected()
    {
        /*
         * SkinnedMeshRenderer renderer = transform.Find("Liam_T_Shirt").gameObject.GetComponent<SkinnedMeshRenderer>();

        Material[] mats = renderer.materials;
        mats[0] = selectedColor;
        renderer.materials = mats;
        */
    }


    public void reset()
    {
        if (materials.Length > 0)
        {
            Material m = materials[Random.Range(0, materials.Length)];
            shirtcolor = m.name;
            SkinnedMeshRenderer renderer = transform.Find("Liam_T_Shirt").gameObject.GetComponent<SkinnedMeshRenderer>();

            Material[] mats = renderer.materials;
            mats[0] = m;
            renderer.materials = mats;

            //Debug.Log("YES" + transform.Find("Liam_T_Shirt").gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].name);
        }
        else
        {

            Debug.Log("No materials for shirts set.");
        }
    }
}
