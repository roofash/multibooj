using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

namespace booj
{

    using Random = UnityEngine.Random;
    [RequireComponent(typeof(AudioSource))]
    public class plrmove : NetworkBehaviour
    {
        

        NetworkManager manager;

        [SerializeField] private GameObject ChatUI = null;
        [SerializeField] private TMP_Text chatText = null;
        [SerializeField] private TMP_InputField inputfield = null;

        private static event Action<string> OnMessage;

        public GameObject ar;
        public GameObject sniper;

        private bool whichgun = true;
        private bool On_Off = true;


        public GameObject onehundred;
        public GameObject ninety;
        public GameObject eighty;
        public GameObject seventy;
        public GameObject sixty;
        public GameObject fifty;
        public GameObject fourty;
        public GameObject thirty;
        public GameObject twenty;
        public GameObject ten;

        public AudioClip jump;
        public AudioClip gunsound;
        public AudioSource audio;

        public TextMesh namescreen;
        public TextMesh nametext;
        public GameObject nametag;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;

        private Material playerMaterialClone;

        public GameObject bullet;
        public Transform shootPoint;
        public float shootSpeed = 15f;
        private float nextTimeToFire = 0f;
        public float fireRate = 10f;

        public float health = 100;

        public CharacterController controller;



        public float speed = 14f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;


        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        Vector3 velocity;
        bool isGrounded;

        public float mouseSensitivity = 100f;


        public Camera playercamera;
        public Transform playerBody;

        float xRotation = 0f;
        float yRotation = 0f;
        float mouseX;
        float mouseY;

        public void Start()
        {
            audio = GetComponent<AudioSource>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            if (!isLocalPlayer)
            {
                playercamera.gameObject.SetActive(false);
            }
        }

        public override void OnStartAuthority()
        {
            ChatUI.SetActive(true);

            OnMessage += HandleNewMessage;
        }


        public void Update()
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                CmdShoot();
                audio.PlayOneShot(gunsound, 0.5f);
            }


            if (!isLocalPlayer)
            {
                nametag.transform.LookAt(Camera.main.transform);
                return;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                audio.PlayOneShot(jump, 1);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = 30f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 14f;
            }


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                controller.height = 1;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                controller.height = 2;
            }


            mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playercamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            #region healthamount
            if (health == 100)
            {
                onehundred.SetActive(true);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 90)
            {
                onehundred.SetActive(false);
                ninety.SetActive(true);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 80)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(true);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 70)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(true);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 60)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(true);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 50)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(true);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 40)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(true);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 30)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(true);
                twenty.SetActive(false);
                ten.SetActive(false);
            }
            if (health == 20)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(true);
                ten.SetActive(false);
            }
            if (health == 10)
            {
                onehundred.SetActive(false);
                ninety.SetActive(false);
                eighty.SetActive(false);
                seventy.SetActive(false);
                sixty.SetActive(false);
                fifty.SetActive(false);
                fourty.SetActive(false);
                thirty.SetActive(false);
                twenty.SetActive(false);
                ten.SetActive(true);
            }

            #endregion

            if (Input.GetKeyDown(KeyCode.V))
            {
                On_Off = !On_Off;
                if (On_Off)
                {
                    Debug.Log("on");
                    audio.enabled = true;

                }
                else
                {
                    Debug.Log("off");
                    audio.enabled = false;
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                whichgun = !whichgun;

                if (whichgun)
                {
                    Debug.Log("sniper");
                    ar.SetActive(false);
                    sniper.SetActive(true);

                    shootSpeed = 70;
                    fireRate = 1;
                }
                else
                {
                    Debug.Log("ar");
                    sniper.SetActive(false);
                    ar.SetActive(true);

                    fireRate = 40;
                    shootSpeed = 70;
                }


            }

            

        }


        [Command]
        void CmdShoot()
        {

            Shoot();

        }
                
        public void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (other.gameObject.tag == "bullet")
            {
                health -= 10;
            }
            if (whichgun)
            {
                health -= 50;
            }
            else
            {
                health -= 10;
            }
            if (health <= 0)
            {
                Debug.Log("ded");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            


        }

        private void OnControllerColliderHit(ControllerColliderHit other)
        {
            if (other.gameObject.tag == "change")
            {
                manager.ServerChangeScene("map2");
                manager.onlineScene = "map2";
                Debug.Log("boww");

            }
        }




        [ClientRpc]
        public void Shoot()
        {


            GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();

            rb.AddForce(shootPoint.forward * shootSpeed, ForceMode.VelocityChange);
        }

        void OnNameChanged(string _Old, string _New)
        {
            nametext.text = playerName;
            namescreen.text = playerName;
        }

        void OnColorChanged(Color _Old, Color _New)
        {
            nametext.color = _New;
            namescreen.color = nametext.color;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            GetComponent<Renderer>().material = playerMaterialClone;
        }

        public override void OnStartLocalPlayer()
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0.881f, 0.151f);

            nametag.transform.localPosition = new Vector3(0.1f, 1.5f, 0);

            nametag.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            name = "LOUG" + Random.Range(100, 999);

            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdSetupPlayer(name, color);
        }

        [Command]
        public void CmdSetupPlayer(string _name, Color _col)
        {

            playerName = _name;
            playerColor = _col;
        }



        [ClientCallback]

        private void OnDestroy()
        {
            if (!hasAuthority)
            {
                return;
            }

            OnMessage -= HandleNewMessage;
        }

        private void HandleNewMessage(string message)
        {
            chatText.text += message;
        }

        [Client]

        public void Send(string message)
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            CmdSendMessage(inputfield.text);

            inputfield.text = string.Empty;
        }

        [Command]

        private void CmdSendMessage(string message)
        {
            RpcHandleMessage($"[{playerName}]: {message}");
        }

        [ClientRpc]

        private void RpcHandleMessage(string message)
        {
            OnMessage?.Invoke($"\n{message}");
        }



    }

}