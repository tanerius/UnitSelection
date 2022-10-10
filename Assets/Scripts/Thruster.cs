using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private Selected m_selected;
    private Rigidbody m_rb;
    public Camera mainCamera;

    public float m_lerpSpeed;
    private Vector3 m_targetPos;
    private float m_xPos;
    private float m_yPos;
    private float m_zPos;
    public LayerMask layerMask;


    public bool xView;
    public bool yView;
    public bool zView;

    public GameObject bgX;
    public GameObject bgY;
    public GameObject bgZ;

   

    public GameObject nearestObject;


    // Start is called before the first frame update
    void Start()
    {
        m_selected = gameObject.GetComponent<Selected>();
        m_targetPos = transform.position;
        m_xPos = gameObject.transform.position.x;
        m_yPos = gameObject.transform.position.y;
        m_zPos = gameObject.transform.position.z;
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nearestObject != null)
        {
            if (xView)
            {
                bgX.transform.position = new Vector3(nearestObject.transform.position.x, transform.position.y, transform.position.z);
            }
            if (yView)
            {
                bgY.transform.position = new Vector3(transform.position.x, nearestObject.transform.position.y, transform.position.z);
            }
            if (zView)
            {
                bgZ.transform.position = new Vector3(transform.position.x, transform.position.y, nearestObject.transform.position.z);
            }
        }
        
        if (m_selected.isSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
            {
                if(xView)
                {
                    m_xPos = transform.position.x;
                    m_targetPos = new Vector3(m_xPos, hit.point.y, hit.point.z);
                }
                if (yView)
                {
                    m_yPos = transform.position.y;
                    m_targetPos = new Vector3(hit.point.x, m_yPos, hit.point.z);
                }
                if (zView)
                {
                    m_zPos = transform.position.z;
                    m_targetPos = new Vector3(hit.point.x, hit.point.y, m_zPos);
                }
            }
            else
            {
                m_targetPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_zPos));
            }
        }

        if(xView)
        {
            transform.position = new Vector3(m_xPos, transform.position.y, transform.position.z);
        }
        if (yView)
        {
            transform.position = new Vector3(transform.position.x, m_yPos, transform.position.z);
        }
        if (zView)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_zPos);
        }
        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
    }
}