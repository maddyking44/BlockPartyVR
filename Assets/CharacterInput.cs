using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterInput : MonoBehaviour
{

    public GameObject Block0;
    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;

    public Vector3 SnapFactors;
    public Vector3 OffsetFactors;
    public GameObject Dot;

    BlocksService blocksService;
    string userId;
    List<GameObject> blocks;

    BlockUtilities blockUtilities;

    Dictionary<int, GameObject> blockTypeToGameObject;

    void Start()
    {
        blocks = new List<GameObject>();
        userId = System.Guid.NewGuid().ToString();
        blocksService = new BlocksService(userId);
        blocksService.BlockAdded += new BlockAddEventHandler(handleBlockAdded);
        blocksService.BlockRemoved += new BlockRemoveEventHandler(handleBlockRemoved);
        blocksService.Start();
        blockTypeToGameObject = new Dictionary<int, GameObject>();
        blockTypeToGameObject[0] = Block0;
        blockTypeToGameObject[1] = Block1;
        blockTypeToGameObject[2] = Block2;
        blockTypeToGameObject[3] = Block3;
        blockTypeToGameObject[4] = Block4;


        blockUtilities = new BlockUtilities();
    }

    void handleBlockAdded(object sender, BlockAddEventArgs args)
    {
        currentBlockType = args.BlockTypeId;
        InstantiateBlock(args.BlockLocation, args.BlockName);
    }

    void handleBlockRemoved(object sender, BlockRemoveEventArgs args)
    {
        blocksService.WriteDebugMessage("handleBlockRemoved");
        RemoveBlock(args.Name);
    }

    GameObject getBlockByName(string name)
    {
        return blocks.Where(b => b.name == name).FirstOrDefault();
    }

    public void RemoveBlock(string name)
    {
        blocksService.WriteDebugMessage("RemoveBlock " + name);
        var block = getBlockByName(name);
        if (block != null)
        {
            blocksService.WriteDebugMessage("Block found");
            blocks.Remove(block);
            blocksService.WriteDebugMessage("Blocked removed from list");
            GameObject.Destroy(block);
            blocksService.WriteDebugMessage("Block destroyed");
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
        string blockName = System.Guid.NewGuid().ToString();
        return InstantiateBlock(position, blockName);
    }

    public GameObject InstantiateBlock(Vector3 position, string blockName)
    {
        if (blockExistsAtPoint(position))
        {
            return null;
        }

        GameObject currentBlock = blockTypeToGameObject[currentBlockType];
        GameObject newBlock = (GameObject)Instantiate(currentBlock, position, Quaternion.identity);
        newBlock.name = blockName;
        newBlock.tag = "my_block";
        blocks.Add(newBlock);

        return newBlock;
    }    

    void placeBlockAtHitPoint(RaycastHit hit)
    {
        if(SnapFactors == null)
            throw new UnityException("SnapFactors should not be null");

        if(OffsetFactors == null)
            throw new UnityException("OffsetFactors should not be null");
            
        Vector3 pointRounded = blockUtilities.CalculateRoundedPoint(hit, SnapFactors, OffsetFactors);
        GameObject newBlock = InstantiateBlock(pointRounded);
        blocksService.WriteBlockToDatabase(newBlock, currentBlockType);
    }

    int currentBlockType = 0;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        for(int i=0; i<5; i++){
            int k = i + 1;
            if (Input.GetKeyUp(k.ToString()))
            {
                currentBlockType = i;
            }
        }

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
