// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.IO.Ports;
// using Newtonsoft.Json;

// public class ArduinoGameController : MonoBehaviour
// {
//     public SerialPort sp = new SerialPort("COM16", 115200); // set port of your arduino connected to computer

//     public int testemove = 0;

//     // Start is called before the first frame update
//     void Start()
//     {
//         sp.Open();
//         sp.ReadTimeout = 1;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (sp.IsOpen && sp.BytesToRead > 0)
//         {
//             try
//             {
//                 string line = sp.ReadLine();

//                 var jsonObject = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(line);

//                 int fsrValue = jsonObject.Value<int>("fsrValue");
//                 int sliderValue = jsonObject.Value<int>("sliderValue");
//                 string action = jsonObject.Value<string>("action");

//                 float ardunioSliderVolume = Map(sliderValue, 1023f, 0f, 0f, 1f);
//                 FindObjectOfType<SoundManager>().volumeSlider.value = ardunioSliderVolume;

//                 float ardunioFSRPressed = Map(fsrValue, 0f, 1023f, 10f, 20f);
//                 if(FindObjectOfType<ShootingItem>() != null){
//                     FindObjectOfType<ShootingItem>().speed = ardunioFSRPressed;
//                 }

//                 switch (action)
//                 {
//                     case "LEFT":
//                         // Debug.Log(action);
//                         testemove = 1;
//                         break;
//                     case "RIGHT":
//                         // Debug.Log(action);
//                         testemove = -1;
//                         break;
//                     case "STOP":
//                         // Debug.Log(action);
//                         testemove = 0;
//                         break;
//                     case "JUMP":
//                         // Debug.Log(action);
//                         FindObjectOfType<PlayerMovement>().jump = true;
//                         FindObjectOfType<PlayerMovement>().animator.SetBool(
//                             "IsJumping",
//                             FindObjectOfType<PlayerMovement>().jump
//                         );
//                         break;
//                     case "ATTACK":
//                         // Debug.Log(action);
//                         if (Time.time >= FindObjectOfType<PlayerMovement>().nextAttackTime)
//                         {
//                             if (FindObjectOfType<Shooting>().canShoot)
//                             {
//                                 StartCoroutine(FindObjectOfType<PlayerMovement>().BowAttack(0f));
//                                 FindObjectOfType<PlayerMovement>().nextAttackTime =
//                                     Time.time + 1.5f;
//                             }
//                             else
//                             {
//                                 FindObjectOfType<PlayerMovement>().Attack();
//                                 FindObjectOfType<PlayerMovement>().nextAttackTime =
//                                     Time.time + 1f / FindObjectOfType<PlayerMovement>().attackRate;
//                             }
//                         }
//                         break;
//                     case "PAUSE":
//                         // Debug.Log(action);
//                         if (FindObjectOfType<PauseMenu>().GameIsPaused)
//                         {
//                             FindObjectOfType<PauseMenu>().Resume();
//                         }
//                         else
//                         {
//                             FindObjectOfType<PauseMenu>().Pause();
//                         }
//                         break;
//                     case "INTERACTION":
//                         // Debug.Log(action);
//                         if (FindObjectOfType<InteractionSystem>().DetectedObject())
//                         {
//                             FindObjectOfType<InteractionSystem>().detectedIObject
//                                 .GetComponent<Item>()
//                                 .Interact();
//                         }
//                         if (FindObjectOfType<DialogueTrigger>().playerDetected)
//                         {
//                             FindObjectOfType<DialogueTrigger>().dialogueScript.StartDialogue();
//                         }
//                         if (FindObjectOfType<Dialogue>().waitForNext)
//                         {
//                             FindObjectOfType<Dialogue>().waitForNext = false;
//                             FindObjectOfType<Dialogue>().index++;

//                             if (
//                                 FindObjectOfType<Dialogue>().index
//                                 < FindObjectOfType<Dialogue>().dialogues.Count
//                             )
//                             {
//                                 // Debug.Log($"Dialogue index: {index}");
//                                 FindObjectOfType<Dialogue>()
//                                     .GetDialogue(FindObjectOfType<Dialogue>().index);
//                             }
//                             else
//                             {
//                                 //End dialogue
//                                 FindObjectOfType<Dialogue>()
//                                     .EndDialogue();
//                                 // Debug.Log(nextLevelAvailable);
//                                 FindObjectOfType<Dialogue>()
//                                     .NextLevelAvailable();
//                             }
//                         }
//                         break;
//                     case "INVENTORY":
//                         // Debug.Log(action);
//                         FindObjectOfType<InventorySystem>().ToggleInventory();
//                         break;
//                     default:
//                         // Debug.Log("switch default: " + action);
//                         break;
//                 }
//             }
//             catch (JsonException e)
//             {
//                 Debug.LogError("Error parsing JSON: " + e.Message);
//             }
//         }

//         // Check for 'B' key press
//         if (Input.GetKeyDown(KeyCode.B))
//         {
//             sp.Write("H"); // Send 'H' character to the Arduino
//         }
//     }

//     public float Map(float value, float minFrom, float maxFrom, float minTo, float maxTo)
// {
//     // First, calculate the normalized position of the value within the input range
//     float normalizedPosition = (value - minFrom) / (maxFrom - minFrom);

//     // Then, calculate the mapped value within the output range
//     float mappedValue = (normalizedPosition * (maxTo - minTo)) + minTo;

//     return mappedValue;
// }
// }
