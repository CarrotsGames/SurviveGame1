using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
public class DSplit : MonoBehaviour {

	public Color bg1;					//colour slot one 
	public Color bg2;					//colour slot two
    PostEffectScript CameraShader;
    GameObject cameraShaderScript;
    GameObject Test;
	private GameObject D1;				//Dimensions 1 group
	private GameObject D2;				//Dimensions 2 group
	public Material yellow;				//colour yellow material
	public Material tYellow;			//colour transparent yellow material
	public Material purple;				//colour purple material
	public Material tPurple;			//colour transparent purple material
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
    void Start() {                      //starts when the game is launched
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");          //finds the "MainCamera" object tag
        D1 = GameObject.FindGameObjectWithTag("D1");                            //finds the "D1" object tag
        D2 = GameObject.FindGameObjectWithTag("D2");                           //finds the "D2" object tag
        cameraShaderScript = GameObject.FindGameObjectWithTag("MainCamera");
        CameraShader = cameraShaderScript.GetComponent<PostEffectScript>();
        Test = GameObject.FindGameObjectWithTag("BackDrop");
        Test.SetActive(false);
        SwitchD (); 															//calls the SwitchD void 
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

	void Update (){ 					//updates ever frame of the game
		if (XCI.GetButtonDown (XboxButton.X)) { 						//if we get xbox butten X on tap

			if (state == 1) {
                CameraShader.enabled = true;
                Test.SetActive(true);
                  state = 2;												//then make it 2
			} else if (state == 2) {
                CameraShader.enabled = false;
                Test.SetActive(false);

                //however if state is already 2
                state = 1;												//then make it 1
			}
				
			SwitchD ();													//call the SwitchD void
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
	private void SwitchD(){
		if (state == 1) {
            // Sets Dimension1 Objects box collider to true 
			for (int i = 0; i < D1.transform.childCount; i++) {


				D1.transform.GetChild (i).GetComponent<BoxCollider2D> ().enabled = true;
				D1.transform.GetChild (i).GetComponent<Renderer> ().material = yellow;
			}
		    // Sets Dimension2 Objects box collider to false
			for (int i = 0; i < D2.transform.childCount; i++) {
				D2.transform.GetChild (i).GetComponent<BoxCollider2D> ().enabled = false;
				D2.transform.GetChild (i).GetComponent<Renderer> ().material = tPurple;

			}	
			cameraObject.GetComponent<Camera> ().backgroundColor = bg1;
			cameraObject.GetComponent<Camera> ().backgroundColor = bg2;

		}
		if (state == 2) {

			for (int i = 0; i < D2.transform.childCount; i++) {


				D2.transform.GetChild (i).GetComponent<BoxCollider2D> ().enabled = true;
				D2.transform.GetChild (i).GetComponent<Renderer> ().material = purple;
			}
			for (int i = 0; i < D1.transform.childCount; i++) {
				D1.transform.GetChild (i).GetComponent<BoxCollider2D> ().enabled = false;
				D1.transform.GetChild (i).GetComponent<Renderer> ().material = tYellow;
			}
			cameraObject.GetComponent<Camera> ().backgroundColor = bg2;
			cameraObject.GetComponent<Camera> ().backgroundColor = bg1;
		}
	}
}