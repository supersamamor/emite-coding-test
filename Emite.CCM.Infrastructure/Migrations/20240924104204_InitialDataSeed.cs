using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emite.CCM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Agent (Id, Email, Name, PhoneExtension, Status, CreatedDate, LastModifiedDate)
                VALUES 
                (NEWID(), 'agent1@example.com', 'Agent 1', '101', 'Available', GETDATE(), GETDATE()),
                (NEWID(), 'agent2@example.com', 'Agent 2', '102', 'Available', GETDATE(), GETDATE()),
                (NEWID(), 'agent3@example.com', 'Agent 3', '103', 'Busy', GETDATE(), GETDATE()),
                (NEWID(), 'agent4@example.com', 'Agent 4', '104', 'Available', GETDATE(), GETDATE()),
                (NEWID(), 'agent5@example.com', 'Agent 5', '105', 'Busy', GETDATE(), GETDATE()),
                (NEWID(), 'agent6@example.com', 'Agent 6', '106', 'Available', GETDATE(), GETDATE()),
                (NEWID(), 'agent7@example.com', 'Agent 7', '107', 'Busy', GETDATE(), GETDATE()),
                (NEWID(), 'agent8@example.com', 'Agent 8', '108', 'Available', GETDATE(), GETDATE()),
                (NEWID(), 'agent9@example.com', 'Agent 9', '109', 'Busy', GETDATE(), GETDATE()),
                (NEWID(), 'agent10@example.com', 'Agent 10', '110', 'Available', GETDATE(), GETDATE());
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Customer (Id, Email, LastContactDate, Name, PhoneNumber, CreatedDate, LastModifiedDate)
                VALUES 
                (NEWID(), 'customer1@example.com', GETDATE(), 'Customer 1', '1111111111', GETDATE(), GETDATE()),
                (NEWID(), 'customer2@example.com', GETDATE(), 'Customer 2', '2222222222', GETDATE(), GETDATE()),
                (NEWID(), 'customer3@example.com', GETDATE(), 'Customer 3', '3333333333', GETDATE(), GETDATE()),
                (NEWID(), 'customer4@example.com', GETDATE(), 'Customer 4', '4444444444', GETDATE(), GETDATE()),
                (NEWID(), 'customer5@example.com', GETDATE(), 'Customer 5', '5555555555', GETDATE(), GETDATE()),
                (NEWID(), 'customer6@example.com', GETDATE(), 'Customer 6', '6666666666', GETDATE(), GETDATE()),
                (NEWID(), 'customer7@example.com', GETDATE(), 'Customer 7', '7777777777', GETDATE(), GETDATE()),
                (NEWID(), 'customer8@example.com', GETDATE(), 'Customer 8', '8888888888', GETDATE(), GETDATE()),
                (NEWID(), 'customer9@example.com', GETDATE(), 'Customer 9', '9999999999', GETDATE(), GETDATE()),
                (NEWID(), 'customer10@example.com', GETDATE(), 'Customer 10', '1010101010', GETDATE(), GETDATE());
            ");


            migrationBuilder.Sql(@"
                INSERT INTO Call (Id, AgentId, CustomerId, EndTime, Notes, StartTime, Status, CreatedDate, LastModifiedDate)
                VALUES 
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer1@example.com'), GETDATE(), 'Call Note 1', GETDATE(), 'Queued', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer2@example.com'), GETDATE(), 'Call Note 2', GETDATE(), 'In Progress', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer3@example.com'), GETDATE(), 'Call Note 3', GETDATE(), 'Completed', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer4@example.com'), GETDATE(), 'Call Note 4', GETDATE(), 'Queued', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer5@example.com'), GETDATE(), 'Call Note 5', GETDATE(), 'In Progress', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer6@example.com'), GETDATE(), 'Call Note 6', GETDATE(), 'Completed', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer7@example.com'), GETDATE(), 'Call Note 7', GETDATE(), 'Queued', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer8@example.com'), GETDATE(), 'Call Note 8', GETDATE(), 'In Progress', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer9@example.com'), GETDATE(), 'Call Note 9', GETDATE(), 'Completed', GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer10@example.com'), GETDATE(), 'Call Note 10', GETDATE(), 'Queued', GETDATE(), GETDATE());
            ");


            migrationBuilder.Sql(@"
                INSERT INTO Ticket (Id, AgentId, CustomerId, CreatedAt, Description, Priority, Resolution, Status, UpdatedAt, CreatedDate, LastModifiedDate)
                VALUES 
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer1@example.com'), GETDATE(), 'Ticket 1', 'Low', NULL, 'Open', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer2@example.com'), GETDATE(), 'Ticket 2', 'Medium', NULL, 'Open', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer3@example.com'), GETDATE(), 'Ticket 3', 'High', NULL, 'Closed', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer4@example.com'), GETDATE(), 'Ticket 4', 'Low', NULL, 'Open', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer5@example.com'), GETDATE(), 'Ticket 5', 'Medium', NULL, 'In Progress', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer6@example.com'), GETDATE(), 'Ticket 6', 'High', NULL, 'Open', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer7@example.com'), GETDATE(), 'Ticket 7', 'Low', NULL, 'Closed', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer8@example.com'), GETDATE(), 'Ticket 8', 'Medium', NULL, 'In Progress', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer9@example.com'), GETDATE(), 'Ticket 9', 'High', NULL, 'Open', GETDATE(), GETDATE(), GETDATE()),
                (NEWID(), NULL, (Select Top 1 Id From Customer where email =  'customer10@example.com'), GETDATE(), 'Ticket 10', 'Low', NULL, 'Closed', GETDATE(), GETDATE(), GETDATE());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
