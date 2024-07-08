CREATE PROCEDURE dbo.User_PasswordUpdate
(
    @UserId NVARCHAR(255),
    @Password NVARCHAR(255) 
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */
BEGIN TRANSACTION

UPDATE Users SET  
    Password = @Password
    WHERE
        UserId = @UserId


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
