namespace PRSProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allownullondateupdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "DateUpdated", c => c.DateTime());
            AlterColumn("dbo.Vendors", "DateUpdated", c => c.DateTime());
            AlterColumn("dbo.PurchaseRequests", "DateUpdated", c => c.DateTime());
            AlterColumn("dbo.Users", "DateUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "DateUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseRequests", "DateUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Vendors", "DateUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Products", "DateUpdated", c => c.DateTime(nullable: false));
        }
    }
}
