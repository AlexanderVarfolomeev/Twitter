using Microsoft.AspNetCore.Identity;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Messenger;
using Twitter.Entities.Tweets;

namespace Twitter.Entities.Users;

public class TwitterUser : IdentityUser<Guid>, IBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = string.Empty;
    public bool IsBanned { get; set; } = false;

    public Guid? AvatarId { get; set; }
    public virtual TwitterFile? Avatar { get; set; }

    public virtual ICollection<TwitterRoleTwitterUser> TwitterRoles { get; set; }
    public virtual ICollection<Tweet> Tweets { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public virtual ICollection<ReportToComment> ReportsToComments { get; set; }

    public virtual ICollection<ReportToTweet> ReportsToTweets { get; set; }

    public virtual ICollection<Message> Messages { get; set; }

    public virtual ICollection<UserLikeTweet> LikeTweets { get; set; }

    public virtual ICollection<Subscribe> Subscribers { get; set; } // подписчики

    public virtual ICollection<Subscribe> Subscribes { get; set; } //подписки

    public virtual ICollection<Dialog> DialogsUser1 { get; set; } // диалоги где юзер = юзер1
    public virtual ICollection<Dialog> DialogsUser2 { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }


    public bool IsNew => Id == Guid.Empty;

    public void Init()
    {
        Id = Guid.NewGuid();
        CreationTime = ModificationTime = DateTime.UtcNow;
    }
}