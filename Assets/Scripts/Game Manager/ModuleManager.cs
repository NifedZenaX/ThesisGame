using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    #region Singleton
    public static ModuleManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public List<ModuleMapping.ModuleTypeEnum> moduleTypeEnumList { get; private set; }
    public List<GameModule> gameModules;
    
    public void SpawnGameModule()
    {
        //TODO: Nanti hapus argument timer di function GetGameObjectFromPool
        //ModuleMapping.ModuleTypeEnum module = GetAvailableModuleTypeEnum();
        //ObjectPool.Instance.GetGameObjectFromPool(module.ToString(), 1);

        foreach (GameModule gm in gameModules)
        {
            gm.gameObject.SetActive(false);
        }

        int moduleIdx = Random.Range(0, gameModules.Count);
        gameModules[moduleIdx].gameObject.SetActive(true);
    }

    public void FinishModuleMock()
    {
        SpawnGameModule();
        ScoreManager.instance.AddSatisfiedCustomer();
    }

    public ModuleMapping.ModuleTypeEnum GetAvailableModuleTypeEnum()
    {
        return moduleTypeEnumList[Random.Range(0, moduleTypeEnumList.Count - 1)];
    }   

    private void Start()
    {
        //moduleTypeEnumList = new List<ModuleMapping.ModuleTypeEnum>();
        //moduleTypeEnumList.Add(ModuleMapping.ModuleTypeEnum.Wires);
        ////moduleTypeEnumList.Add(ModuleMapping.ModuleTypeEnum.Module_Upgrade);
        //moduleTypeEnumList.Add(ModuleMapping.ModuleTypeEnum.Shapes_Numbers);
        //moduleTypeEnumList.Add(ModuleMapping.ModuleTypeEnum.Forgot_Password);

        Time.timeScale = 1;
        SpawnGameModule();
    }
}
