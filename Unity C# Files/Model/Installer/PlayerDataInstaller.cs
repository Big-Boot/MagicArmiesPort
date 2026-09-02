using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;
using System;
using UnityEngine;

public class PlayerDataInstaller : AbstractInstaller
{
    #region Public Methods

    public override void Install(ServiceLocator serviceLocator)
    {
        try
        {
            // build your config
            var gameSaveFile = new FBPPConfig()
            {
                SaveFilePath = Application.dataPath,
                //EncryptionSecret = "kyngegambit-tovictory",
                //ScrambleSaveData = true,
                ScrambleSaveData = false,
                OnLoadError = loadBackup,
            };
            // pass it to FBPP
            FBPP.Start(gameSaveFile);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        PlayerData playerData;
        if (FBPP.GetString("PlayerData", "") == "")
        {
            playerData = new PlayerData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(FBPP.GetString("PlayerData", ""));
        }

        serviceLocator.RegisterService<IPlayerData>(playerData);
        playerData.Initialize(this);
    }

    public static void ResetData()
    {
        ServiceLocator serviceLocator = ServiceLocator.Instance;
        serviceLocator.UnregisterService<IPlayerData>();
        MonoBehaviour owner = PlayerData.instance.owner;
        PlayerData.instance.Dispose();
        PlayerData playerData;
        if (FBPP.GetString("PlayerData", "") == "")
        {
            playerData = new PlayerData();
        }
        else
        {
            playerData = JsonUtility.FromJson<PlayerData>(FBPP.GetString("PlayerData", ""));
        }

        serviceLocator.RegisterService<IPlayerData>(playerData);
        playerData.Initialize(owner);
    }

    public void loadBackup()
    {
        // build your config

        var config = new FBPPConfig()
        {
            SaveFilePath = Application.dataPath,
            AutoSaveData = true,
            //EncryptionSecret = "kyngegambit-tovictory",
            //ScrambleSaveData = true,
            ScrambleSaveData=false,
            SaveFileName = "Backup",
        };
        // pass it to FBPP
        FBPP.Start(config);
        string data = FBPP.GetSaveFileAsJson();

        // build your config
        config = new FBPPConfig()
        {
            SaveFilePath = Application.dataPath,
            //EncryptionSecret = "kyngegambit-tovictory",
            ScrambleSaveData=false
            //ScrambleSaveData = true
        };
        // pass it to FBPP
        FBPP.Start(config);
        FBPP.OverwriteLocalSaveFile(data);
    }

    #endregion Public Methods
}