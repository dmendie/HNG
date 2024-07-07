CREATE PROCEDURE [dbo].[User_GetOrganisations]
(
	@UserId NVARCHAR(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */


SELECT
	o.OrgId,
	o.Name,
	o.Description,
    o.Status,
	o.CreatedOn,
	o.CreatedBy,
	o.ModifiedOn,
	o.ModifiedBy
FROM 
	Organisation o
	LEFT OUTER JOIN UserOrganisation uo
		ON o.OrgId = uo.OrgId
WHERE
	uo.UserId = @UserId
ORDER BY
    o.Name


/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR


RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/

