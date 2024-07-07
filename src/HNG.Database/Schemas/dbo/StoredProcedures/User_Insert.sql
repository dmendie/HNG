CREATE PROCEDURE dbo.User_Insert
(
    @UserId NVARCHAR(255),
	@Firstname NVARCHAR(50),
    @Lastname NVARCHAR(50),
    @Phone NVARCHAR(15),
    @Email NVARCHAR(255),
    @Status int,
    @Password NVARCHAR(255) null,
	@CreatedBy NVARCHAR(100)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int,
    @OrgId nvarchar = CAST(NEWID() AS NVARCHAR(255)),
    @FormattedFirstname NVARCHAR(100)

/* Beginning of procedure */
BEGIN TRANSACTION

 -- Format the firstname
    SET @FormattedFirstname = dbo.CapitalizeFirstLetter(@Firstname) + '''s Organisation';

INSERT INTO Users 
(
    UserId,
    FirstName,
    LastName,
    Phone,
    Email,
    Status,
	CreatedOn,
	CreatedBy,
	ModifiedOn,
	ModifiedBy,
    Password
)
VALUES
(
    @UserId,
    @Firstname,
    @Lastname,
    @Phone,
    @Email,
    @Status,
	GETUTCDATE(),
	@CreatedBy,
	GETUTCDATE(),
	@CreatedBy,
    @Password

)

INSERT INTO Organisation
(
    OrgId,
    Name,
    Description,
    CreatedOn,
	CreatedBy,
	ModifiedOn,
	ModifiedBy
)
VALUES
(
    @OrgId,
    @FormattedFirstname,
    'Your first organisation',
    GETUTCDATE(),
	@CreatedBy,
	GETUTCDATE(),
	@CreatedBy
)

INSERT INTO UserOrganisation
(
    UserId,
    OrgId,
    IsOwner,
	CreatedOn,
	CreatedBy,
	ModifiedOn,
	ModifiedBy
)
VALUES
(
    @UserId,
    @OrgId,
    1,
    GETUTCDATE(),
	@CreatedBy,
	GETUTCDATE(),
	@CreatedBy
)

/* error-handling */
SELECT @error = @@error
IF (@error <> 0) GOTO ERROR

COMMIT TRANSACTION
RETURN 0

/* error-handling */
ERROR:
	IF (@@trancount > 0) ROLLBACK TRANSACTION
	RETURN @error

/* End of procedure*/
