CREATE PROCEDURE dbo.Organisation_AddUser
(
	@UserId NVARCHAR(255),
	@OrgId NVARCHAR(255),
	@CreatedBy NVARCHAR(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */
BEGIN TRANSACTION

IF NOT EXISTS (SELECT 1 FROM UserOrganisation WHERE UserId = @UserId AND OrgId = @OrgId)
    BEGIN
        
		INSERT INTO UserOrganisation
		(
			UserId,
			OrgId,
			CreatedOn,
			CreatedBy,
			ModifiedOn,
			ModifiedBy
		)
		VALUES
		(
			@UserId,
			@OrgId,
			GETUTCDATE(),
			@CreatedBy,
			GETUTCDATE(),
			@CreatedBy
		)

    END


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