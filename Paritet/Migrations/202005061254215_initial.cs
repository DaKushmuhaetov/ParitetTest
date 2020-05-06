namespace Paritet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "Height", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "Height", c => c.Double(nullable: false));
        }
    }
}
