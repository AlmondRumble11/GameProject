using System.Collections.Generic;
using UnityEngine;

public class UpgradeManagerScript : MonoBehaviour
{

    public List<Upgrade> upgrades = new List<Upgrade>(){
            new Upgrade()
            {
                Type = UpgradesTypes.DrainSoul,
                Name = "Drain soul",
                Description = "Get health back after each projectile hit",
                Value = 2,
            },

            new Upgrade()
            {
                Type = UpgradesTypes.AreaShot,
                Name = "Pyroblast",
                Description = "Projectile that has area effect",
                Value = 5,
            },

            new Upgrade()
            {
                Type = UpgradesTypes.UpgradedProjectile,
                Name = "Flames of Uldun",
                Description = "Projectile goes thorugh enemies",
                Value = 0,
            },
            new Upgrade()
            {
                Type = UpgradesTypes.FullHealth,
                Name = "Tree of life",
                Description = "Fully heal",
                Value = 0,
            },
            new Upgrade()
            {
                Type = UpgradesTypes.SpinningProjectile,
                Name = "Mana zone",
                Description = "Automatic spinning projectile",
                Value = 30,
            },
          new Upgrade()
          {
              Type = UpgradesTypes.AbilityIncrease,
              Name = "Chosen by the gods",
              Description = "Increase health, speed and damage by 1.5x",
              Value = 1.5f,

          }
    };
    public List<NormalUpgrade> normalUpgrades = new List<NormalUpgrade>()
        {
            new NormalUpgrade()
            {
                Name = "Health",
                Value = 15,
                Type = NormalUpgradesTypes.Health,
                Price = 0

            },
            new NormalUpgrade()
            {
                Name = "Speed",
                Value = 0.5f,
                Type = NormalUpgradesTypes.Speed,
                Price = 0
            },
            new NormalUpgrade()
            {
                Name = "Damage",
                Value = 5,
                Type = NormalUpgradesTypes.Damage,
                Price = 0
            }

        };
    public Projectile ProjectileValues = new Projectile()
    {
        Damage = 10,
        Type = ProjectileType.Normal
    };

    public List<NormalUpgrade> portalStoreUpgrades = new List<NormalUpgrade>() {
        new NormalUpgrade()
        {
            Name = "+50 Health",
            Price = 10,
            Type = NormalUpgradesTypes.Health,
            Value = 50
        },
        new NormalUpgrade()
        {
            Name = "+2 Speed",
            Price = 30,
            Type = NormalUpgradesTypes.Speed,
            Value = 2f
        },
        new NormalUpgrade()
        {
            Name = "1.5x Damage",
            Price = 50,
            Type = NormalUpgradesTypes.Damage,
            Value = 1.5f
        }
    };


    public void AddDamage(int damageValue)
    {
        ProjectileValues.Damage += damageValue;
    }
}

public class Projectile
{
    public ProjectileType Type;
    public int Damage;

}

public class Upgrade
{
    public UpgradesTypes Type;
    public string Name;
    public float Value;
    public string Description;

}

public class NormalUpgrade
{
    public NormalUpgradesTypes Type;
    public string Name;
    public float Value;
    public int Price;
}


public enum NormalUpgradesTypes
{
    Health,
    Damage,
    Speed
}


public enum UpgradesTypes
{
    DrainSoul,
    AreaShot,
    UpgradedProjectile,
    SpinningProjectile,
    FullHealth,
    AbilityIncrease
}

public enum ProjectileType
{
    Normal,
    AreaShot,
    Upgraded
}

