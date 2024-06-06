using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorEvents : MonoBehaviour
{
    public GameObject doorFrame;

    // Material properties.
    /*
    private int numMaterials;
    private Color[] originalColors;
    private Color highlightColor = Color.red;
    private MeshRenderer objectMeshRenderer;
    */

    // Door outlines.
    private Outline doorOutline;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch mesh renderer.
        // this.objectMeshRenderer = GetComponent<MeshRenderer>();

        // Fetch original color of the object.
        /*
        this.numMaterials = this.objectMeshRenderer.materials.Length;
        this.originalColors = new Color[this.numMaterials];
        for(int idxMaterial = 0; idxMaterial < this.numMaterials; idxMaterial++)
        {
            this.originalColors[idxMaterial] = this.objectMeshRenderer.materials[idxMaterial].color;
        }
        */

        // Setup outlines.
        this.doorOutline = gameObject.GetComponent<Outline>();
        this.doorOutline.enabled = false;
        // this.doorOutline.initialized = true;

        Debug.Log("[Event] Door events set up.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Highlight the door when raycasting it.
    /*
    void OnMouseOver()
    {
        for(int idxMaterial = 0; idxMaterial < this.numMaterials; idxMaterial++)
        {
            // this.objectMeshRenderer.materials[idxMaterial].color = this.highlightColor;
        }
        this.doorOutline.enabled = true;
        this.frameOutline.enabled = true;
    }

    // Reset color when removing raycasting.
    void OnMouseExit()
    {
        for(int idxMaterial = 0; idxMaterial < this.numMaterials; idxMaterial++)
        {
            // this.objectMeshRenderer.materials[idxMaterial].color = this.originalColors[idxMaterial];
        }
        this.doorOutline.enabled = false;
        this.frameOutline.enabled = false;
    }
    */
}
