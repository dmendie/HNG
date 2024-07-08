CREATE PROCEDURE dbo.Organisation_Insert
(
	@UserId NVARCHAR(255),
	@Name NVARCHAR(200),
	@Description NVARCHAR(1000),
	@CreatedBy NVARCHAR(255),
	@OrgId NVARCHAR(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */
BEGIN TRANSACTION

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
    @Name,
    @Description,
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

