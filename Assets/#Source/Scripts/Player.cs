using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public AudioClip goldClip;
    public AudioClip deathClip;
    //public AudioClip test;
    public GameObject zoomPosition;
	public int jumpUp = 400;


	private Animator playerAnimtor;
	private Rigidbody2D rig2D;
	private int runCount;
    private bool isOver;
    private bool shield;

    private Vector3 oldPosition;
    private float oldCameraSize;
    private Camera mainCamera;
	private GameMenu menu;
	// Use this for initialization
	void Start () {
        shield = false;
		playerAnimtor = this.GetComponent<Animator> ();
		rig2D = this.GetComponent<Rigidbody2D> ();
        playerAnimtor.SetBool("cap", false);

        GameObject temp;
		if ((temp = GameObject.Find ("GamePanel")) != null) {
			menu = temp.GetComponent<GameMenu>();
		}



        mainCamera = Camera.main;
        oldCameraSize = Camera.main.orthographicSize;
        

		GameInfo.RestGameInfo();
        StartCoroutine(AddMi());



	}

    public IEnumerator AddMi() 
    {
        while(this.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(1f);
            GameInfo.mi++;
        }
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && runCount != 2&&isOver==false) {
			runCount++;

            rig2D.AddForce(new Vector2(0, jumpUp));

			if(runCount == 2){
				playerAnimtor.SetBool ("run02",true);
			}
			playerAnimtor.SetBool ("run01",true);
		}

        if (Camera.main.WorldToScreenPoint(transform.position).x < -10 && isOver ==  false)
        {
            isOver = true;
            Debug.Log("角色死亡");
            AudioSource.PlayClipAtPoint(deathClip, transform.position, 1f);

            StartCoroutine(IEGameOver(1.5f));
        }


        if (zoomPosition != null)
        {
            if (transform.position.y >= zoomPosition.transform.position.y)
            {
                Camera.main.orthographicSize = oldCameraSize + (transform.position.y - zoomPosition.transform.position.y) * 0.5f;
            }
        }

        oldPosition = transform.position;

	    
    }

	public void OnCollisionEnter2D(Collision2D solider)
	{

		if(solider.gameObject.CompareTag(Tags.Floor))
		{
			//Debug.Log("碰撞地面");
			runCount = 0;
			playerAnimtor.SetBool ("run01",false);
			playerAnimtor.SetBool ("run02",false);
		}
	}

	public void OnTriggerEnter2D(Collider2D solider){

		if(solider.gameObject.CompareTag(Tags.Gold))
		{
			//Debug.Log("碰撞到金币");
            GameInfo.coinNum++;
			solider.gameObject.SetActive(false);
			Destroy(solider.gameObject,2);
			
			AudioSource.PlayClipAtPoint(goldClip,this.transform.position,1f);
		}

        if (solider.gameObject.CompareTag(Tags.evil)&&isOver==false)
        {
            //Debug.Log("碰撞到怪物");
            if (shield)
            {
                shield = false;
                playerAnimtor.SetBool("cap", false);
            }
            else
            {
                isOver = true;
                AudioSource.PlayClipAtPoint(deathClip, transform.position, 1f);
                StartCoroutine(IEGameOver(0.5f));
            }
        }

        if (solider.gameObject.CompareTag(Tags.DeathFloor) && isOver == false) 
        {
            isOver = true;
           // Debug.Log("触发到死亡区域");
            AudioSource.PlayClipAtPoint(deathClip, transform.position, 1f);
            StartCoroutine(IEGameOver(1.5f));
        }
        if(solider.gameObject.CompareTag(Tags.TransferGate) && isOver == false)
        {
            GameInfo.mi += 10;
            solider.gameObject.SetActive(false);
            Destroy(solider.gameObject, 2);
        }
        if (solider.gameObject.CompareTag(Tags.Shield) && isOver == false)
        {
            solider.gameObject.SetActive(false);
            shield = true;
            Destroy(solider.gameObject, 2);
            playerAnimtor.SetBool("cap", true);
        }
    }



    private IEnumerator IEGameOver(float timer)
    {
        //倒转摄像机
        while (mainCamera.orthographicSize >=  -4.5f) 
        {
            yield return new WaitForFixedUpdate();
            mainCamera.orthographicSize -= Time.deltaTime * 4.5f;
            //Debug.Log("开始倒转摄像机: " + mainCamera.orthographicSize);
        }

        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
		menu.ShowGameOver ();
		//Application.LoadLevel("Start");
    }

}
