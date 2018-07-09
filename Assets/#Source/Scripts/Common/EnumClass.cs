using UnityEngine;
using System.Collections;



public class Tags
{
	public static string Gold = "Gold";
	public static string Floor = "Floor";
    public static string DeathFloor = "DeathFloor";
    public static string evil = "evil";
    public static string TransferGate = "TransferGate";
    public static string Shield = "Shield";
}

public enum CreateType 
{
    /// <summary>
    /// 按时间来生成物体
    /// </summary>
    Time,
    /// <summary>
    /// 按物体到达某一个位置,在生成下一个物体
    /// </summary>
    ObjEnd
}