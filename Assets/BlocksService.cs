
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;






public class BlocksService
{
    DatabaseReference databaseReference;
    public event BlockAddEventHandler BlockAdded;
    public event BlockRemoveEventHandler BlockRemoved;

    string _userId;

    public BlocksService(string userId)
    {
        _userId = userId;
    }

    public void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(AppSettings.FireBaseUrl);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child("blocks").ChildAdded += HandleBlocksAdded;
        databaseReference.Child("blocks").ChildRemoved += HandleBlocksRemoved;


        this.WriteDebugMessage("User " + _userId + " started");
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
        if(BlockAdded != null){
            BlockAdded(this, new BlockAddEventArgs(aPoint));
        }
    }

    void HandleBlocksRemoved(object sender, ChildChangedEventArgs args)
    {
        this.WriteDebugMessage("a - HandleBlocksRemoved");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        this.WriteDebugMessage("b - HandleBlocksRemoved - b");

        if(BlockRemoved != null){
            this.WriteDebugMessage("c - HandleBlocksRemoved");
            BlockRemoved(this, new BlockRemoveEventArgs(args.Snapshot.Key));
        }

    }

    public void WriteBlockToDatabase(GameObject aBlock)
    {
        if (aBlock == null)
            return;

        string blockData = string.Format("block|{0}|{1}|{2}|{3}|{4}",
            aBlock.transform.position.x,
            aBlock.transform.position.y,
            aBlock.transform.position.z,
            0,
            _userId
        );

        databaseReference.Child("blocks").Child(aBlock.name).SetValueAsync(blockData);
    }

     public void WriteDebugMessage(string message)
    {
        if (message == null)
            return;

        databaseReference.Child("debug_log").Push().SetValueAsync(message);
    }   

    public void RemoveBlockFromDatabase(string blockId)
    {
        var blockNode = databaseReference.Child("/blocks/" + blockId);
        blockNode.RemoveValueAsync();
    }
}