
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class BlocksService
{
    DatabaseReference databaseReference;
    CharacterInput _characterInputBehavior;
    string _userId;

    public BlocksService(CharacterInput characterInputBehavior, string userId){
        _characterInputBehavior = characterInputBehavior;
        _userId = userId;
    }

    public void Start(){
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(AppSettings.FireBaseUrl);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        databaseReference.Child("timestamp").SetValueAsync(System.DateTime.Now.ToString());
        databaseReference.Child("blocks").ChildAdded += HandleBlocksAdded;
        databaseReference.Child("blocks").ChildRemoved += HandleBlocksRemoved;
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
        _characterInputBehavior.InstantiateBlock(aPoint);
    }

    void HandleBlocksRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        _characterInputBehavior.RemoveBlock(args.Snapshot.Key);
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

    public void RemoveBlockFromDatabase(string blockId)
    {
        var blockNode = databaseReference.Child("/blocks/" + blockId);
        blockNode.RemoveValueAsync();
    }
}