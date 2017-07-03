namespace PersonalSpendingAnalysis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSHA256FieldToTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BankName = c.String(),
                        AccountNumber = c.String(),
                        AccountName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Imports",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Filename = c.String(),
                        importDate = c.DateTime(nullable: false),
                        SHA256 = c.String(),
                        Account_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        transactionDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        SHA256 = c.String(),
                        Category_Id = c.Guid(),
                        Account_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategorySearchStrings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        String = c.String(),
                        Category_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.CategorySearchStrings", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Imports", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.CategorySearchStrings", new[] { "Category_Id" });
            DropIndex("dbo.Transactions", new[] { "Account_Id" });
            DropIndex("dbo.Transactions", new[] { "Category_Id" });
            DropIndex("dbo.Imports", new[] { "Account_Id" });
            DropTable("dbo.CategorySearchStrings");
            DropTable("dbo.Categories");
            DropTable("dbo.Transactions");
            DropTable("dbo.Imports");
            DropTable("dbo.Accounts");
        }
    }
}
