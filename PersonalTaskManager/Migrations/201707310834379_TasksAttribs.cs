namespace PersonalTaskManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksAttribs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        Owner_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Users", t => t.Owner_UserId, cascadeDelete: true)
                .Index(t => t.Owner_UserId);
            
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Owner_UserId", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "Owner_UserId" });
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "UserName", c => c.String());
            DropTable("dbo.Tasks");
        }
    }
}
