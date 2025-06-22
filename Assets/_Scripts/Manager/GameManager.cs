using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("Settings")]
    public bool friendlyFire;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Re-link player and UI
        player = FindObjectOfType<Player>();
        UI.instance = FindObjectOfType<UI>();
    }

    public void GameStart()
    {
        SetDefaultWeaponsForPlayer();
        LevelGenerator.instance.InitializeLevelPart();
        // We start selected the mission in a levelGenerator scripts after we done the level creation.
    }

    public void RestartScene()
    {
        Destroy(UI.instance?.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void GameOver()
    {
        UI.instance.ShowGameOverUI();
        CameraManager.instance.ChangeCameraDistance(3);
    }

    public void GameWinner()
    {
        UI.instance.ShowGameWinner();
    }
    public void SetDefaultWeaponsForPlayer()
    {
        List<Weapon_Data> newWeaponDataList = UI.instance.weaponSelectionUI.SelectedWeaponData();
        player.weapon.SetDefaultWeapon(newWeaponDataList);
    }  
}
