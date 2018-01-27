using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterInput : MonoBehaviour
{

    public GameObject MyBlock;
    public Vector3 SnapFactors;
    public Vector3 OffsetFactors;
    public GameObject Dot;

    BlocksService blocksService;
    string userId;
    List<GameObject> blocks;

    void Start()
    {
        blocks = new List<GameObject>();
        userId = System.Guid.NewGuid().ToString();
        blocksService = new BlocksService(userId);
        blocksService.BlockAdded += new BlockAddEventHandler(handleBlockAdded);
        blocksService.BlockRemoved += new BlockRemoveEventHandler(handleBlockRemoved);
        blocksService.Start();
    }

    void handleBlockAdded(object sender, BlockAddEventArgs args)
    {
        InstantiateBlock(args.BlockLocation);
    }

    void handleBlockRemoved(object sender, BlockRemoveEventArgs args)
    {
        RemoveBlock(args.Name);
    }

    GameObject getBlockByName(string name)
    {
        return blocks.Where(b => b.name == name).FirstOrDefault();
    }

    public void RemoveBlock(string name)
    {
        var block = getBlockByName(name);
        if (block != null)
        {
            blocks.Remove(block);
            GameObject.Destroy(block);
        }
    }

    bool blockExistsAtPoint(Vector3 aPoint)
    {
        foreach (var block in blocks)
        {
            var blockPosition = block.transform.position;
            if (aPoint.x == blockPosition.x &&
                aPoint.y == blockPosition.y &&
                aPoint.z == blockPosition.z)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject InstantiateBlock(Vector3 position)
    {
        if (blockExistsAtPoint(position))
        {
            return null;
        }

        GameObject newBlock = (GameObject)Instantiate(MyBlock, position, Quaternion.identity);
        newBlock.name = System.Guid.NewGuid().ToString();
        newBlock.tag = "my_block";
        blocks.Add(newBlock);

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

        GameObject newBlock = InstantiateBlock(pointRounted);
        blocksService.WriteBlockToDatabase(newBlock);
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(ray, out hit))
        {
            Dot.SetActive(true);
            Dot.transform.position = hit.point;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                placeBlockAtHitPoint(hit);
            }
            else if (Input.GetKeyUp(KeyCode.Delete))
            {
                var block = hit.collider.gameObject;
                if (block.tag == "my_block")
                {
                    Debug.Log("Delete object!");
                    blocks.Remove(block);
                    Destroy(block);
                    blocksService.RemoveBlockFromDatabase(block.name);
                }
            }
        }
        else
        {
            Dot.SetActive(false); ;
        }
    }
}
