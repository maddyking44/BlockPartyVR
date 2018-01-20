using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public int BlockType { get; set; }
    public string CreatedBy { get; set; }
}

public class CharacterInput : MonoBehaviour
{

    public GameObject MyBlock;
    public Vector3 SnapFactors;
    public Vector3 OffsetFactors;
    List<Vector3> blockPoints;

    public GameObject Dot;

    DatabaseReference databaseReference;

    string userId;

    // Use this for initialization
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(AppSettings.FireBaseUrl);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        blockPoints = new List<Vector3>();

        userId = System.Guid.NewGuid().ToString();

        databaseReference.Child("timestamp").SetValueAsync(System.DateTime.Now.ToString());
        databaseReference.Child("blocks").ChildAdded += HandleBlocksAdded;
    }

    void HandleBlocksAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        string blockString = args.Snapshot.Value.ToString();
        string[] blockArray = blockString.Split('|');
        float x = float.Parse(blockArray[1]);
        float y = float.Parse(blockArray[2]);
        float z = float.Parse(blockArray[3]);
        string blockUserId = blockArray[4];

        Vector3 aPoint = new Vector3(x, y, z);
        instantiateBlock(aPoint);
    }

    bool blockExistsAtPoint(Vector3 aPoint)
    {
        foreach (Vector3 p in blockPoints)
        {
            if (aPoint.x == p.x && aPoint.y == p.y && aPoint.z == p.z)
            {
                return true;
            }
        }

        return false;
    }

    GameObject instantiateBlock(Vector3 position)
    {
        if (position.y < 0 && !blockExistsAtPoint(position))
        {
            return null;
        }

        GameObject newBlock = (GameObject)Instantiate(MyBlock, position, Quaternion.identity);
        newBlock.name = System.Guid.NewGuid().ToString();
        newBlock.tag = "my_block";
        blockPoints.Add(position);

        return newBlock;
    }

    void placeBlockAtHitPoint(RaycastHit hit)
    {
        Vector3 point = hit.point;
        Vector3 MyNormal = hit.normal;

        //this will convert that normal from being relative to global axis to relative to an
        //objects local axis

        MyNormal = hit.transform.TransformDirection(MyNormal);

        //this next line will compare the normal hit to the normals of each plane to find the 
        //side hit
        float partialX = point.x / SnapFactors.x - Mathf.Floor(point.x / SnapFactors.x);
        float partialY = point.y / SnapFactors.y - Mathf.Floor(point.y / SnapFactors.x);
        float partialZ = point.z / SnapFactors.z - Mathf.Floor(point.z / SnapFactors.x);
        Vector3 pointRounted = new Vector3();

        if (MyNormal == hit.transform.up)
        {
            Debug.Log("top");
            if (partialX > 0.5f)
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            else
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.up) //important note the use of the '-' sign this inverts the direction, -up == down. Down doesn't exist as a stored direction, you invert up to get it. 
        {
            Debug.Log("bottom");
            if (partialX > 0.5f)
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            else
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y - OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == hit.transform.right)
        {
            Debug.Log("hit from right");
            pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.right) //note the '-' sign converting right to left
        {
            Debug.Log("hit from left");
            pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.forward)
        {
            Debug.Log("hit from forward");
            if (partialX > 0.5f)
            {
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }
            else
            {
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);

        }
        else if (MyNormal == hit.transform.forward)
        {
            Debug.Log("hit from behind");
            if (partialX > 0.5f)
            {
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }
            else
            {
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
        }

        GameObject newBlock = instantiateBlock(pointRounted);
        writeBlockToDatabase(newBlock);
    }


    void writeBlockToDatabase(GameObject aBlock)
    {
        if (aBlock == null)
            return;

        string blockData = string.Format("block|{0}|{1}|{2}|{3}|{4}",
            aBlock.transform.position.x,
            aBlock.transform.position.y,
            aBlock.transform.position.z,
            0,
            userId
        );

        databaseReference.Child("blocks").Child(aBlock.name).SetValueAsync(blockData);
    }

    void removeBlockFromDatabase(string blockId)
    {
        var blockNode = databaseReference.Child("/blocks/" + blockId);
        blockNode.RemoveValueAsync();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit))
        {
            Dot.SetActive(true);
            Dot.transform.position = hit.point;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                placeBlockAtHitPoint(hit);
            }
            else if (Input.GetKeyDown("c"))
            {
                var block = hit.collider.gameObject;
                if (block.tag == "my_block")
                {
                    Debug.Log("Delete object!");
                    Destroy(block);
                    removeBlockFromDatabase(block.name);
                }
            }
        }
        else
        {
            Dot.SetActive(false); ;
        }
    }
}
