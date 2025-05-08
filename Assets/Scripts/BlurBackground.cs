using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurBackground : MonoBehaviour
{
    public Volume globalVolume;
    public float focusDistance = 0f; // Adjust this value to control the focus distance.

    private DepthOfField depthOfField;

    private void Start()
    {
       
        if (globalVolume == null)
        {
            return;
        }

    
        if (globalVolume.profile.TryGet(out DepthOfField dof))
        {
            depthOfField = dof;
            depthOfField.focusDistance.value = focusDistance;
        }
        
    }

    public async void Unblur()
    {
        if (globalVolume.profile.TryGet(out DepthOfField dof))
        {
            for (int i = 0; i < 100; i++)
            {
                depthOfField.focusDistance.value += 0.05f;
                await Task.Delay(15);
            }
        }
    }

    public async void Blur() 
    {
        if (globalVolume.profile.TryGet(out DepthOfField dof))
        {
            for (int i = 0; i < 100; i++)
            {
                depthOfField.focusDistance.value -= 0.05f;
                await Task.Delay(15);
            }
        }

   
    }


}
