using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DistortionEffect : MonoBehaviour
{
    private Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material == null)
        {
            Shader s = Shader.Find("Hidden/DistortionShader");
            material = new(s);
        }
        Graphics.Blit(source, destination, material);        
    }
}
