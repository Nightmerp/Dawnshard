namespace DragaliaAPI.Shared.Definitions.Enums;

public enum FriendStatus
{
    Friend = 0,
    RequestByViewerID1 = 1, // ViewerID1 requests to be friends with ViewerID2
    RequestByViewerID2 = 2 // ViewerID2 requests to be friends with ViewerID1
}
