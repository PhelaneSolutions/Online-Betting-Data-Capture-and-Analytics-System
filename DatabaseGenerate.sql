
-- Create CasinoWagers Table
CREATE TABLE CasinoWagers (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    WagerId UNIQUEIDENTIFIER NOT NULL,
    AccountId UNIQUEIDENTIFIER NOT NULL,
    Theme NVARCHAR(50),
    Provider NVARCHAR(50),
    GameName NVARCHAR(50),
    TransactionId UNIQUEIDENTIFIER,
    BrandId UNIQUEIDENTIFIER,
    ExternalReferenceId UNIQUEIDENTIFIER,
    TransactionTypeId UNIQUEIDENTIFIER,
    Amount DECIMAL(18, 2) NOT NULL,
    CreatedDateTime DATETIME2 NOT NULL,
    NumberOfBets INT,
    SessionData NVARCHAR(MAX),
    Duration INT,
    CountryCode NVARCHAR(4000),
    Username NVARCHAR(4000),
);

-- Create Indexes
CREATE INDEX IX_CasinoWagers_AccountId ON CasinoWagers (AccountId);
CREATE INDEX IX_CasinoWagers_CreatedDateTime ON CasinoWagers (CreatedDateTime);

-- Stored Procedure to Insert Casino Wager
CREATE PROCEDURE InsertCasinoWager
    @WagerId UNIQUEIDENTIFIER,
    @AccountId UNIQUEIDENTIFIER,
    @Theme NVARCHAR(50),
    @Provider NVARCHAR(50),
    @GameName NVARCHAR(50),
    @TransactionId UNIQUEIDENTIFIER,
    @BrandId UNIQUEIDENTIFIER,
    @ExternalReferenceId UNIQUEIDENTIFIER,
    @TransactionTypeId UNIQUEIDENTIFIER,
    @Amount DECIMAL(18, 2),
    @CreatedDateTime DATETIME2,
    @NumberOfBets INT,
    @SessionData NVARCHAR(MAX),
    @Duration INT,
    @CountryCode NVARCHAR(400),
    @Username NVARCHAR(400)
AS
BEGIN
    INSERT INTO CasinoWagers (WagerId, AccountId, Theme, Provider, GameName, TransactionId, BrandId, ExternalReferenceId, TransactionTypeId, Amount, CreatedDateTime, NumberOfBets, SessionData, Duration)
    VALUES (@WagerId, @AccountId, @Theme, @Provider, @GameName, @TransactionId, @BrandId, @ExternalReferenceId, @TransactionTypeId, @Amount, @CreatedDateTime, @NumberOfBets, @SessionData, @Duration);
END;

-- Stored Procedure to Get Player Casino Wagers
CREATE PROCEDURE GetPlayerCasinoWagers
    @PlayerId UNIQUEIDENTIFIER,
    @PageSize INT,
    @Page INT
AS
BEGIN
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT WagerId, GameName AS Game, Provider, Amount, CreatedDateTime AS CreatedDate
    FROM CasinoWagers
    WHERE AccountId = @PlayerId
    ORDER BY CreatedDateTime DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) AS Total
    FROM CasinoWagers
    WHERE AccountId = @PlayerId;
END;

-- Stored Procedure to Get Top Spenders
CREATE PROCEDURE GetTopSpenders
    @Count INT
AS
BEGIN
    SELECT TOP (@Count) p.AccountId, p.Username, SUM(cw.Amount) AS TotalAmountSpend
    FROM CasinoWagers p
    JOIN CasinoWagers cw ON p.AccountId = cw.AccountId
    GROUP BY p.AccountId, p.Username
    ORDER BY TotalAmountSpend DESC;
END;