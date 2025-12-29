using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    
    // 玩家数据
    public int playerHealth;
    public int playerMaxHealth;
    public SerializableVector3 playerPosition;
    
    // 可以添加更多数据
    // public int playerLevel;
    // public List<string> unlockedSkills;
    // 等等...

    public GameData()
    {
        this.currency = 0;
        this.playerHealth = 100;
        this.playerMaxHealth = 100;
        this.playerPosition = new SerializableVector3(0, 0, 0);
    }
}

// Unity的Vector3不能直接序列化为JSON，需要创建一个可序列化的版本
[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
    
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    
    public static SerializableVector3 FromVector3(Vector3 vector)
    {
        return new SerializableVector3(vector.x, vector.y, vector.z);
    }
}
