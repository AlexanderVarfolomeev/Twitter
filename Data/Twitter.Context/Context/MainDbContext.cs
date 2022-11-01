using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Messenger;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;

namespace Twitter.Context.Context;

public class MainDbContext : IdentityDbContext<TwitterUser, TwitterRole, Guid>
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) { }
    
    public DbSet<TwitterUser> Users { get; set; }
    public DbSet<TwitterRole> Roles { get; set; }
    public DbSet<Subscribe> Subscribes { get; set; }
    public DbSet<TwitterRoleTwitterUser> TwitterRolesTwitterUsers { get; set; }

    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<UserLikeTweet> LikeTweets { get; set; }
    public DbSet<ReportToTweet> ReportsToTweets { get; set; }
    public DbSet<FileTweet> FileTweets { get; set; }

    public DbSet<Dialog> Dialogs { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageFile> MessageFiles { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<FileComment> FileComments { get; set; }
    public DbSet<ReportToComment> ReportsToComments { get; set; }
    
    public DbSet<ReasonReport>  ReasonReports { get; set; }
    public DbSet<TwitterFile> TwitterFiles { get; set; }
//TODO настроить
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tweet>().HasKey(x => x.Id);
        builder.Entity<TwitterUser>().HasKey(x => x.Id);
        builder.Entity<TwitterRole>().HasKey(x => x.Id);
        builder.Entity<Subscribe>().HasKey(x => x.Id);
        builder.Entity<UserLikeTweet>().HasKey(x => x.Id);
        builder.Entity<ReportToTweet>().HasKey(x => x.Id);
        builder.Entity<ReportToComment>().HasKey(x => x.Id);
        builder.Entity<FileTweet>().HasKey(x => x.Id);
        builder.Entity<Dialog>().HasKey(x => x.Id);
        builder.Entity<Message>().HasKey(x => x.Id);
        builder.Entity<MessageFile>().HasKey(x => x.Id);
        builder.Entity<Comment>().HasKey(x => x.Id);
        builder.Entity<FileComment>().HasKey(x => x.Id);
        builder.Entity<ReasonReport>().HasKey(x => x.Id);
        builder.Entity<TwitterFile>().HasKey(x => x.Id);

        #region Tweets

        builder.Entity<Tweet>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.Tweets)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Tweet>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Tweet)
            .HasForeignKey(x => x.TweetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Tweet>()
            .HasMany(x => x.Reports)
            .WithOne(x => x.Tweet)
            .HasForeignKey(x => x.TweetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<FileTweet>().HasOne(x => x.File)
            .WithMany(x => x.Tweets)
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FileTweet>().HasOne(x => x.Tweet)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.TweetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserLikeTweet>().HasOne(x => x.Tweet)
            .WithMany(x => x.Likes)
            .HasForeignKey(x => x.TweetId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<UserLikeTweet>().HasOne(x => x.User)
            .WithMany(x => x.LikeTweets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Comments

        builder.Entity<Comment>().HasMany(x => x.Reports)
            .WithOne(x => x.Comment)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>().HasOne(x => x.Creator)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FileComment>().HasOne(x => x.File)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FileComment>().HasOne(x => x.Comment)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region ReasonReport

        builder.Entity<ReasonReport>().HasMany(x => x.Comments)
            .WithOne(x => x.Reason)
            .HasForeignKey(x => x.ReasonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ReasonReport>().HasMany(x => x.Tweets)
            .WithOne(x => x.Reason)
            .HasForeignKey(x => x.ReasonId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        builder.Entity<TwitterUser>().HasOne(x => x.Avatar)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.AvatarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TwitterUser>().HasMany(x => x.ReportsToTweets)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TwitterUser>().HasMany(x => x.ReportsToComments)
            .WithOne(x => x.Creator)
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Subscribe>().HasOne(x => x.Subscriber)
            .WithMany(x => x.Subscribes)
            .HasForeignKey(x => x.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Subscribe>().HasOne(x => x.User)
            .WithMany(x => x.Subscribers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Dialog>().HasOne(x => x.FirstUser)
            .WithMany(x => x.DialogsUser2)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.FirstUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Dialog>().HasOne(x => x.SecondUser)
            .WithMany(x => x.DialogsUser1)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(x => x.SecondUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Dialog>().HasMany(x => x.Messages)
            .WithOne(x => x.Dialog)
            .HasForeignKey(x => x.DialogId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TwitterUser>().HasMany(x => x.Messages)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<MessageFile>().HasOne(x => x.File)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<MessageFile>().HasOne(x => x.Message)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TwitterRoleTwitterUser>().HasOne(x => x.User)
            .WithMany(x => x.TwitterRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<TwitterRoleTwitterUser>().HasOne(x => x.Role)
            .WithMany(x => x.TwitterUsers)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);



    }
}