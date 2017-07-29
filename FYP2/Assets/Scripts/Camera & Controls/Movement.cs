using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    public float speed;
    public VirtualJoystick joystick;
    Vector2 tileSize;
    Vector3 waypoint;
    public bool m_input;//this booleaan decides wether the game can take movement commands from the player(disable during ransitions,cutscenes whatnot)

    Vector3 up;
    Vector3 down;
    Vector3 left;
    Vector3 right;


    bool moving = false;

    void Start()
    {
        //Tiled2Unity.TiledMap Map = GameObject.Find("starting_area").GetComponent<Tiled2Unity.TiledMap>();
        //tileSize.Set(Map.TileWidth, Map.TileHeight);
        //up.Set(0, tileSize.y, 0);
        //down.Set(0, -tileSize.y, 0);
        //left.Set(-tileSize.x, 0, 0);
        //right.Set(tileSize.x, 0, 0);
        m_input = true;
        //setting speed
        speed = 500;
    }

	// Update is called once per frame
	void Update () {

        //if (moving == true)
        //{
        //    if (waypoint - this.gameObject.transform.position != Vector3.zero)
        //        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, waypoint, tileSize.x * speed * Time.deltaTime);
        //    else
        //        moving = false;
        //}
        if (!m_input)
            return;
        if(joystick.ismoving)
        {
            moveplayer(joystick.GetStickDirection(),  joystick.GetStickMagnitude());
        }
        else if(!GetKeyboardInput())
        {
            //play idle animation in correct direction? stop sprite anim for now in correct direction
            this.gameObject.GetComponent<Animator>().speed = 0;
        }

    }

    public void moveplayer(Vector3 direction ,float magnitude = 1)//control-based movement
    {
        //Debug.Log(direction);//the below line isnt necessary for just 4 directions
        Vector3 initialpos = gameObject.transform.position;
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, this.gameObject.transform.position + direction.normalized, magnitude * speed*Time.deltaTime);
        SharedData.instance.EncounterManager.CheckEncounter((initialpos - transform.position).sqrMagnitude);
        UpdateAnimations(direction);
    }

    public bool forcemoveplayer(Vector3 point)//forces the plaayer to move in a direction until they reach the destination, then return a boolean(turn off m_input to fully "force")
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,point,0.08f*speed * Time.deltaTime);//player seems to be moving according to frame rate, needs fix
        if (this.gameObject.transform.position == point)
        {
            return true;//if arrived at destination return true
        }
        else
            return false;
        //UpdateAnimations(direction);

    }


    public void UpdateAnimations(Vector3 dir)
    {
        ResetSpriteDirection();
        if(dir.x != 0)//east/west
        {
            if(dir.x > 0)//traveling east
            {
                this.gameObject.GetComponentInParent<Animator>().SetBool("east", true);
                return;
            }
            else//traveling west
            {
                this.gameObject.GetComponentInParent<Animator>().SetBool("west", true);
                return;
            }
 
        }
        if (dir.y != 0)//enorth/south
        {
            if (dir.y > 0)//traveling north
            {
                this.gameObject.GetComponentInParent<Animator>().SetBool("north", true);
                return;
            }
            else//south
            {
                this.gameObject.GetComponentInParent<Animator>().SetBool("south", true);
                return;
            }

        }

        //if the above dont trigger , player is standing still
    }
    
    public void changeArea(TransitionArea node)//move the plaayer into a new area
    {
        //Debug.Log("moving player");
        this.gameObject.transform.position = node.transform.position;
    }
    public void ResetSpriteDirection()
    {
        this.gameObject.GetComponentInParent<Animator>().speed = 1;
        this.gameObject.GetComponentInParent<Animator>().SetBool("north", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("south", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("east", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("west", false);
    }
    bool GetKeyboardInput()
    {
            if (Input.GetKey(KeyCode.Z))
            {

                SharedData.instance.EncounterManager.GenerateEncounter();
                //Application.LoadLevelAdditive("YourNextScene"); //loads your desired other scene
            }


            if (Input.GetKey(KeyCode.W))
            {
                moveplayer(new Vector3(0, 1, 0));
                return true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveplayer(new Vector3(-1, 0, 0));
                //waypoint = this.gameObject.transform.position + left;
                //moving = true;
                return true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveplayer(new Vector3(0, -1, 0));
                //waypoint = this.gameObject.transform.position + down;
                //moving = true;
                return true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveplayer(new Vector3(1, 0, 0));
                //waypoint = this.gameObject.transform.position + right;
                //moving = true;
                return true;
            }
            return false;

    }

}
