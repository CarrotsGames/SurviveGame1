using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class DSplitScript : MonoBehaviour
{
    public Color bg1;                   //colour slot one 
    public Color bg2;                   //colour slot two
    GameObject PlayerGameObj;
    GameObject PlayerGameObj2;

    [HideInInspector]
    public int LEVELTRACK = -1;

    GameObject cameraShaderScript;
    GameObject backDrop;
    private GameObject D1;              //Dimensions 1 group
    private GameObject D2;
    public Material DimensioSwapMat;
    //Dimensions 2 group
    public Material yellow;             //colour yellow material
    public Material tYellow;            //colour transparent yellow material
    public Material purple;             //colour purple material
    public Material tPurple;            //colour transparent purple material
    private int state = 1;				//Dimensional state equals 1 
    public bool dimensionShift;
    private GameObject cameraObject;    //camera
                                        //------------------------------------------------------------------------------------------------------------------
                                        //		Start()
                                        // Runs during initialisation
                                        //
                                        // Param:
                                        //				None
                                        // Return:
                                        //				Void
                                        //------------------------------------------------------------------------------------------------------------------
    void Start()
    {                      //starts when the game is launched
        PlayerGameObj = GameObject.FindGameObjectWithTag("Player");
        PlayerGameObj2 = GameObject.FindGameObjectWithTag("Player2");

        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");          //finds the "MainCamera" object tag
        D1 = GameObject.FindGameObjectWithTag("D1");                            //finds the "D1" object tag
        D2 = GameObject.FindGameObjectWithTag("D2");                           //finds the "D2" object tag
        cameraShaderScript = GameObject.FindGameObjectWithTag("MainCamera");
        // backDrop = GameObject.FindGameObjectWithTag("BackDrop");
        // backDrop.SetActive(false);
        dimensionShift = true;
        SwitchD();                                                          //calls the SwitchD void 
    }
    //------------------------------------------------------------------------------------------------------------------
    //		Update()
    // Runs every frame
    //
    // Param:
    //				None
    // Return:
    //				Void
    //------------------------------------------------------------------------------------------------------------------

    void Update()
    {                   //updates ever frame of the game
        if (XCI.GetButtonDown(XboxButton.X))
        {                       //if we get xbox butten X on tap

            if (state == 1)
            {
                //  CameraShader.enabled = true;
                //   backDrop.SetActive(true);
                state = 2;                                              //then make it 2
            }
            else if (state == 2)
            {
                //   CameraShader.enabled = false;
                //    backDrop.SetActive(false);

                //however if state is already 2
                state = 1;                                              //then make it 1
            }

            SwitchD();                                                  //call the SwitchD void
        }

    }
    //-----------------------------------------------------------------------------------------------------------------
    // SwitchD();
    // checks with you are in state 1 then graps all children of tag D1 and turns on D1 tags colliders and changes the 
    // material to yellow. Then grabs all of the D2 tags children and turns off their colliders and changes the material   
    // to transparent purple. switchs the background colours around. does ves versa for state two.
    // Param:
    //				None
    // Return:
    //				Void
    //-----------------------------------------------------------------------------------------------------------------
    private void SwitchD()
    {
        if (state == 1)
        {
            dimensionShift = true;

            // Disables Player in dimension two 
            D2.transform.GetChild(0).GetComponentInChildren<NEWPLAYERSCRIPT>().enabled = false;
            // Changes player twos layer to obstacle
            D2.transform.GetChild(0).gameObject.layer = 9;
            //Changes player1s material to the yellow material
            PlayerGameObj.GetComponent<Renderer>().material = yellow;

            // Sets Dimension1 Objects box collider to true 
            for (int i = 0; i < D1.transform.childCount; i++)
            {

                D1DimensionController();

                // Enables Dimension one box colliders 
                D1.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
                // changes all dimension one objects to the yellow mat
                D1.transform.GetChild(i).GetComponent<Renderer>().material = yellow;
                // Enables players BoxCollider 
                D1.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            }
            // Sets Dimension2 Objects box collider to false
            for (int i = 0; i < D2.transform.childCount; i++)
            {
                D1DimensionController();
                // Disables Dimension two box colliders 
                D2.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
                // Changes all dimension two objects to the tPurple mat
                D2.transform.GetChild(i).GetComponent<Renderer>().material = tPurple;
                // Gets player2s box collider and enables it 
                D2.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            }
            cameraObject.GetComponent<Camera>().backgroundColor = bg1;
            cameraObject.GetComponent<Camera>().backgroundColor = bg2;

        }
        if (state == 2)
        {
            dimensionShift = false;

            // Disables Player in dimension one 
            D1.transform.GetChild(0).GetComponentInChildren<NEWPLAYERSCRIPT>().enabled = false;
            PlayerGameObj.transform.GetChild(0).GetComponent<SpearThrowScript>().enabled = false;
            //Changes player1s layer to obstacle
            D1.transform.GetChild(0).gameObject.layer = 9;
            // Changes Player2s material the yellow mat
            PlayerGameObj2.GetComponent<Renderer>().material = yellow;

            // Sets Dimension2 Objects box collider to true 
            for (int i = 0; i < D2.transform.childCount; i++)
            {

                D2DimensionController();
                // Enables Dimension two box colliders 
                D2.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
                // Changes all dimension two objects to the Purple mat
                D2.transform.GetChild(i).GetComponent<Renderer>().material = purple;
                // Gets player2s box collider and enables it 
                D2.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

            }
            for (int i = 0; i < D1.transform.childCount; i++)
            {
                D2DimensionController();
                // Disables Dimension one box colliders 
                D1.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
                // Changes all dimension one objects to the tPurple mat
                D1.transform.GetChild(i).GetComponent<Renderer>().material = tYellow;
                // Gets player1s box collider and enables it 
                D1.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

            }
            cameraObject.GetComponent<Camera>().backgroundColor = bg2;
            cameraObject.GetComponent<Camera>().backgroundColor = bg1;

        }
    }

    private void D1DimensionController()
    {
        // Enables the player in dimension one to move
        D1.transform.GetChild(0).GetComponentInChildren<NEWPLAYERSCRIPT>().enabled = true;
        PlayerGameObj.transform.GetChild(0).GetComponent<SpearThrowScript>().enabled = true;

        // Enables Controller2D script in parent of PlayerGameObj Mesh 
        PlayerGameObj.GetComponentInParent<NEWPLAYERSCRIPT>().enabled = true;
        // Transforms players layer to Player in scene
        D1.transform.GetChild(0).gameObject.layer = 11;
        // Changes players twos mesh into the DimensionSwapMat
        PlayerGameObj2.GetComponent<Renderer>().material = DimensioSwapMat;


    }
    private void D2DimensionController()
    {
        // Enables the player in dimension two to move
        D2.transform.GetChild(0).GetComponentInChildren<NEWPLAYERSCRIPT>().enabled = true;
        // Enables Controller2D script of PlayerGameObj2  
        PlayerGameObj2.GetComponent<NEWPLAYERSCRIPT>().enabled = true;
        // Transforms players layer to Player in scene
        D2.transform.GetChild(0).gameObject.layer = 11;
        // Changes players ones mesh into the DimensionSwapMat
        PlayerGameObj.GetComponent<Renderer>().material = DimensioSwapMat;


    }

}

