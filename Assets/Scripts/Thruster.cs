using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private Selected m_selected;
    public Camera mainCamera;

    public float m_lerpSpeed;
    private Vector3 m_targetPos;
    private Vector3 m_targetPosStored;
    private float m_xPos;
    private float m_yPos;
    private float m_zPos;
    public LayerMask layerMask;
    public LayerMask layerMask2;

    public float m_rotationSpeed;

    public bool xView;
    public bool yView;
    public bool zView;

    public GameObject bgX;
    public GameObject bgY;
    public GameObject bgZ;

    public GameObject nearestObject;

    public Vector3 startPos;
    public float startRotY;

    public Vector3 rotPos;
    public bool rotationDone = false;

    public Rigidbody rb;
    private Vector3 lastPos;

    public bool isMoving;
    public bool uTurn;

    // Start is called before the first frame update
    void Start()
    {
        m_selected = gameObject.GetComponent<Selected>();
        m_targetPos = transform.position;
        m_xPos = gameObject.transform.position.x;
        m_yPos = gameObject.transform.position.y;
        m_zPos = gameObject.transform.position.z;

        rotationDone = false;
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

        if (m_selected.isSelected && Input.GetMouseButtonDown(1) && !isMoving)
        {
            startPos = transform.position;
            startRotY = transform.eulerAngles.y;
            rotationDone = false;
            isMoving = true;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
            {
                if (xView)
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
            m_targetPosStored = m_targetPos;
        }



        if ((m_targetPos - transform.position).normalized != Vector3.zero && isMoving)
        {
            if (xView)
            {
                if (startPos.z < m_targetPos.z && startRotY < 90)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0, 0, 0)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else if (startPos.z > m_targetPos.z && startRotY > 90)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.Euler(0, 180, 0)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    uTurn = true;

                    if (startPos.z < m_targetPos.z && startRotY > 90)
                    {
                        rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                        if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                        {
                            transform.RotateAround(rotPos, new Vector3(1, 0, 0), 200 * Time.deltaTime);
                        }
                        else
                        {
                            rotationDone = true;
                            transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);

                            if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                            {
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            }
                        }
                    }
                    else
                    {
                        rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                        if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                        {
                            transform.RotateAround(rotPos, new Vector3(-1, 0, 0), 200 * Time.deltaTime);
                        }
                        else
                        {
                            rotationDone = true;
                            transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), 5 * Time.deltaTime);

                            if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                            {
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            }
                        }
                    }

                }
            }
            if (yView)
            {
                if (startPos.x < m_targetPos.x && startRotY < 90)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (bgY.transform.rotation * Quaternion.Euler(0, 90, 90)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else if (startPos.x > m_targetPos.x && startRotY > 90)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (bgY.transform.rotation * Quaternion.Euler(0, 270, -90)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    uTurn = true;

                    if (startPos.x < m_targetPos.x && startRotY > 90)
                    {
                        rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                        if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                        {
                            transform.RotateAround(rotPos, new Vector3(0, 0, -1), 200 * Time.deltaTime);
                        }
                        else
                        {
                            rotationDone = true;
                            transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), 5 * Time.deltaTime);

                            if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                            {
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            }
                        }
                    }
                    else
                    {
                        if (startPos.x < m_targetPos.x && startRotY > 180)
                        {
                            rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                            if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                            {
                                transform.RotateAround(rotPos, new Vector3(0, 0, -1), 200 * Time.deltaTime);
                            }
                            else
                            {
                                rotationDone = true;
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), 5 * Time.deltaTime);

                                if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                                {
                                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                                }
                            }
                        }
                        else
                        {
                            rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                            if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                            {
                                transform.RotateAround(rotPos, new Vector3(0, 0, 1), 200 * Time.deltaTime);
                            }
                            else
                            {
                                rotationDone = true;
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 270, 0), 5 * Time.deltaTime);

                                if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                                {
                                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                                }
                            }
                        }
                    }
                }
            }
            if (zView)
            {
                if (startPos.x < m_targetPos.x && startRotY < 180)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (bgY.transform.rotation * Quaternion.Euler(0, 90, 90)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else if (startPos.x > m_targetPos.x && startRotY > 180)
                {
                    uTurn = false;

                    transform.rotation = Quaternion.Slerp(transform.rotation, (bgY.transform.rotation * Quaternion.Euler(0, 270, -90)), m_rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                        transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    uTurn = true;

                    if (startPos.x < m_targetPos.x && startRotY > 180)
                    {
                        rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                        if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                        {
                            transform.RotateAround(rotPos, new Vector3(0, 0, -1), 200 * Time.deltaTime);
                        }
                        else
                        {
                            rotationDone = true;
                            transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), 5 * Time.deltaTime);

                            if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                            {
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            }
                        }
                    }
                    else
                    {
                        rotPos = new Vector3(startPos.x, startPos.y + 1.5f, startPos.z);

                        if (transform.position.y < rotPos.y + 1.45f && !rotationDone)
                        {
                            transform.RotateAround(rotPos, new Vector3(0, 0, 1), 200 * Time.deltaTime);
                        }
                        else
                        {
                            rotationDone = true;
                            transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 270, 0), 5 * Time.deltaTime);

                            if (Vector3.Distance(transform.position, m_targetPos) > ((Vector3.Distance(startPos, m_targetPos)) / 4))
                            {
                                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((m_targetPos - transform.position).normalized), Time.deltaTime * m_rotationSpeed);
                                transform.position = Vector3.Lerp(transform.position, m_targetPos, m_lerpSpeed * Time.deltaTime);
                            }
                        }
                    }
                }
            }

            Ray ray2 = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray2, out RaycastHit hit2, float.MaxValue, layerMask2))
            {
                if (hit2.transform.position.y <= transform.position.y)
                {
                    m_targetPos.y += 1;
                }
                else
                {
                    m_targetPos.y -= 1;
                }

            }
            else
            {
                m_targetPos = m_targetPosStored;
            }
        }

        var curPos = transform.position;
        if (curPos == lastPos)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;

            if (Vector3.Distance(curPos, m_targetPos) < 0.05)
            {
                transform.position = m_targetPos;
            }
        }
        lastPos = curPos;
    }
}