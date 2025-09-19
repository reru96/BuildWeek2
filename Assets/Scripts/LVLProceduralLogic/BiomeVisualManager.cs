using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BiomeVisualManager : MonoBehaviour
{
    public static BiomeVisualManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Aggiorna visual settings quando cambia bioma
    /// </summary>
    public void ApplyBiomeVisuals(BiomeData biome)
    {
        if (biome == null) return;

        // --- Skybox ---
        if (biome.skyboxMaterial != null)
        {
            RenderSettings.skybox = biome.skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }

        // --- Fog + Ambient ---
        RenderSettings.fogColor = biome.fogColor;
        RenderSettings.ambientLight = biome.ambientLightColor;

        // --- Forza la camera a usare Skybox ---
        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.Skybox;

            if (Camera.main.TryGetComponent<UniversalAdditionalCameraData>(out var camData))
            {
                // qui NON puoi toccare clearDepth, ma puoi settare altre opzioni
                camData.renderPostProcessing = true;
                camData.renderShadows = true;
            }
        }
    }
}


