namespace PersonalSpendingAnalysis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
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
                    CategoryId = c.Guid(),
                    SubCategory = c.String(),
                    AccountId = c.Guid(),
                    SHA256 = c.String(),
                    ManualCategory = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Accounts", t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.AccountId);

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(),
                    SearchString = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Guid(),
                        AccountId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);

            CreateTable(
                "dbo.CategorySearchStrings",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    String = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Imports", "Account_Id", "dbo.Accounts");
            DropIndex("dbo.Budgets", new[] { "CategoryId" });
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.Imports", new[] { "Account_Id" });
            DropTable("dbo.CategorySearchStrings");
            DropTable("dbo.Budgets");
            DropTable("dbo.Categories");
            DropTable("dbo.Transactions");
            DropTable("dbo.Imports");
            DropTable("dbo.Accounts");
        }
    }
}
