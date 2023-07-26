using System.Collections.Generic;
using UnityEngine;

// https://gamedev.stackexchange.com/questions/110958/what-is-the-proper-way-to-handle-data-between-scenes
// Basically stores values as static so that they dont reset between scenes changes
public static class GameData
{
    public static float time;
    public static bool fullRun = true;
    public static int score = 0;
    public static int playerLevel = 1;
    public static List<Upgrade> playerUpgrades = new List<Upgrade>() { };
    public static float playerMovementSpeed = 10f;
    public static int playerMaxHealth = 200000;
    public static int playerHealth = 0;
    public static Projectile currentPlayerProjectile = new Projectile()
    {
        Damage = 100,
        Type = ProjectileType.Upgraded
    };
    public static bool spinnerIsActive = false;
}

// Default values which are used to reset the GameData values after the gameover screen
public static class GameDataDefault
{
    public static float time;
    public static bool fullRun = false;
    public static int score = 0;
    public static int playerLevel = 1;
    public static List<Upgrade> playerUpgrades = new List<Upgrade>() { };
    public static float playerMovementSpeed = 10f;
    public static int playerMaxHealth = 200;
    public static int playerHealth = 0;
    public static Projectile currentPlayerProjectile = new Projectile()
    {
        Damage = 10,
        Type = ProjectileType.Normal
    };
    public static bool spinnerIsActive = false;
}

public class GameDataManager : MonoBehaviour
{
}
