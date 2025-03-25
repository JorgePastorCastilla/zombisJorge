using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    private float cameraSpeed = 300f;
        [SerializeField]
        private Transform playerTransform;
        private float xRotation = 0f;
        private float yRotation = 0f;
        // Start is called before the first frame update
        
        public PhotonView photonView;
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        // Update is called once per frame
        void Update()
        {
            if (PhotonNetwork.InRoom && !photonView.IsMine)
            {
                return;
            }
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed;
            // playerTransform.Rotate(Vector3.up * mouseX);
            yRotation += mouseX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 75f);
            playerTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            
    
    
        }
}
