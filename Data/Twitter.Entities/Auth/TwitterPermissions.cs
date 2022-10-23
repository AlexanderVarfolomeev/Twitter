namespace Twitter.Entities.Auth;

[Flags]
//TODO добавить RoleManager и UserManager 
public enum TwitterPermissions
{
    None        =   0x0000,
    //tweets
    ReadTweet   =   0x0001,
    CreateTweet =   0x0002,
    DeleteTweet =   0x0004,
    UpdateTweet =   0x0008,
    FullAccessTweet = ReadTweet | CreateTweet | DeleteTweet | UpdateTweet, 
    
    //comments
    ReadComment =   0x0010,
    CreateComment = 0x0020,
    DeleteComment = 0x0040,
    UpdateComment = 0x0080,
    FullAccessComment = ReadComment | CreateComment | DeleteComment | UpdateComment,
    
    //accounts
    ReadAccount =   0x0100,
    CreateAccount = 0x0200,
    DeleteAccount = 0x0400,  
    UpdateAccount = 0x0800,
    FullAccessAccount =  ReadAccount | CreateAccount | DeleteAccount | UpdateAccount,
    
    //messages
    ReadMessage =   0x1000,
    CreateMessage = 0x2000,
    DeleteMessage = 0x4000,  
    UpdateMessage = 0x8000,
    FullAccessMessage  =  ReadMessage | DeleteMessage | UpdateMessage | CreateMessage,
    
    FullAccess = FullAccessAccount | FullAccessComment | FullAccessMessage | FullAccessTweet
}