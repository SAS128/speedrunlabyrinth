using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

	private MazeCell currentCell;

	private MazeDirection currentDirection;
    public Text actions;
    public int totalactions = 0;
    public bool f = false;
    string rootURL = "https://speedrunlabyrinth.000webhostapp.com/";
    
    bool submittingScore = false;
  
    
    public void SetLocation (MazeCell cell) {
		if (currentCell != null) {
			currentCell.OnPlayerExited();
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
        currentCell.OnPlayerEntered();

        
    }

    private void Move (MazeDirection direction) {
        if(!f)
        {

            actions = GameObject.Find("Actions").GetComponent<Text>();
            actions.GetComponent<Text>().color = Color.red;
            
            f = true;
        }
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            SetLocation(edge.otherCell);
        }
        totalactions += 1;
        actions.GetComponent<Text>().text = "Number of actions: "+ totalactions;
    }

    private void Look (MazeDirection direction) {
        if (!f)
        {

            actions = GameObject.Find("Actions").GetComponent<Text>();
            actions.GetComponent<Text>().color = Color.red;
           
            f = true;
        }
        totalactions += 1;
        actions.GetComponent<Text>().text = "Number of actions: " + totalactions;
        transform.localRotation = direction.ToRotation();
        currentDirection = direction;
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);

        }
       
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    Debug.Log("SubmitScoreemail" + PlayerPrefs.GetString("userEmail"));
        //    Debug.Log("SubmitScoretotalactions" + totalactions);
        //    StartCoroutine(SubmitScore());
            
        //}
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(currentDirection);
           
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(currentDirection.GetNextClockwise());

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(currentDirection.GetOpposite());
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(currentDirection.GetNextCounterclockwise());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Look(currentDirection.GetNextCounterclockwise());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Look(currentDirection.GetNextClockwise());
        }
    }
    void OnCollisionEnter(Collision otherObj)
    {
        //print(otherObj.gameObject.name);
        if (otherObj.gameObject.tag == "cube")
        {
            Destroy(gameObject, .5f);
            //Debug.Log(PlayerPrefs.GetString("userEmail"));
            Debug.Log("SubmitScoreemail" + PlayerPrefs.GetString("userEmail"));
            Debug.Log("SubmitScoretotalactions" + totalactions);
            StartCoroutine(SubmitScore());
            SceneManager.LoadScene(1);

        }
    }
    void OnTriggerEnter(Collider otherObj)
    {
        //print(otherObj.gameObject.name);
        if (otherObj.gameObject.tag == "cube")
        {
            Destroy(gameObject, .5f);
            Debug.Log("SubmitScoreemail" + PlayerPrefs.GetString("userEmail"));
            Debug.Log("SubmitScoretotalactions" + totalactions);
            StartCoroutine(SubmitScore());
            SceneManager.LoadScene(1);
        }
    }
    IEnumerator SubmitScore()
    {
        
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerPrefs.GetString("userEmail").ToString());
        form.AddField("score", totalactions);
        Debug.Log("form" + form);
        using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "score_submit.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                print(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;

                if (responseText.StartsWith("Success"))
                {
                    print("New Score Submitted!");
                }
                else
                {
                    print(responseText);
                }
            }
        }

       
    }
    
}