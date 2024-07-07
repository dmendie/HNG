CREATE PROCEDURE [dbo].[Organisation_GetByIdOnly]
(
	@OrgId NVARCHAR(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */


SELECT
	u.OrgId,
	u.Name,
	u.Description,
    u.Status,
	u.CreatedOn,
	u.CreatedBy,
	u.ModifiedOn,
	u.ModifiedBy
FROM 
	Organisation u
WHERE
	u.OrgId = @OrgId


/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR


RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/

