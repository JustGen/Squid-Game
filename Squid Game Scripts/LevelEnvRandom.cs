using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnvRandom : MonoBehaviour
{
    public static LevelEnvRandom S;

    [SerializeField] private List<Material> _listOfSkyBoxMat;
    [SerializeField] private List<Material> _listOfPlatformMat;
    [SerializeField] private MeshRenderer _platform;

    private void Awake()
    {
        S = this;
    }

    public void ChangeMaterialsInScene()
    {
        int idVersionEnv = Random.Range(0, 5);
        //int idVersionEnv = 4;
        _platform.material = _listOfPlatformMat[idVersionEnv];
        RenderSettings.skybox = _listOfSkyBoxMat[idVersionEnv];
    }
}
