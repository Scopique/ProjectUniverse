using UnityEngine;
using System.Collections;

public class JumpgateController : MonoBehaviour
{

    #region Inspector Properties

    public GameObject egressPoint;

    #endregion

    void Awake()
    {
        MeshRenderer mr = egressPoint.GetComponent<MeshRenderer>();
        if (mr != null) { mr.enabled = false; }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
