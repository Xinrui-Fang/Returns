using UnityEngine;
using System.Collections;

public class GameCreate : MonoBehaviour {


	public GameObject [] aryFloor;
	public Transform createPosition;
    public CreateType type;

    public float createTime;
    private float createTimer;

	private int createCount;
	private Transform end;
	private GameObject currentObj;

	void Update () {

        if(type == CreateType.Time)
        {
            createTimer += Time.deltaTime;

            if (createTimer >= createTime) 
            {
                createTimer = 0;
                currentObj = (GameObject)GameObject.Instantiate(aryFloor[Random.Range(0, aryFloor.Length)], transform.position, Quaternion.identity);
            }
        }

        if(type == CreateType.ObjEnd)
        {
            if (createCount == 0)
            {
                //timer = 0;
                createCount++;
                currentObj = (GameObject)GameObject.Instantiate(aryFloor[Random.Range(0, aryFloor.Length)], transform.position, Quaternion.identity);
                end = currentObj.transform.FindChild("End");
            }

            #region 到达某一个点控制生成地板
            if (end.position.x < createPosition.position.x)
            {
                currentObj = (GameObject)GameObject.Instantiate(aryFloor[Random.Range(0, aryFloor.Length)], transform.position, Quaternion.identity);
                end = currentObj.transform.FindChild("End");
            }
            #endregion
        }




	}
}

