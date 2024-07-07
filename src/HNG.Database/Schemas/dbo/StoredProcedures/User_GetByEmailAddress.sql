CREATE PROCEDURE dbo.User_GetByEmailAddress
(
	@EmailAddress nvarchar(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */


SELECT
	u.UserId,
	u.Phone,
	u.Password,
	u.FirstName,
	u.LastName,
	u.Email,
    u.Status,
	u.CreatedOn,
	u.CreatedBy,
	u.ModifiedOn,
	u.ModifiedBy
FROM 
	Users u
WHERE
	u.Email = @EmailAddress


/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR


RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/
