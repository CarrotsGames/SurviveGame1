using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour {


    DSplit Dimension;
    public Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
            Graphics.Blit(source, destination, mat);
         
         
    }
}
