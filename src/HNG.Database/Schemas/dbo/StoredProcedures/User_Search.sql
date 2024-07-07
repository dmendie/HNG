CREATE PROCEDURE dbo.User_Search
(
    @SearchUserId nvarchar(255),
    @UserId nvarchar(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
    @error int

/* Beginning of procedure */

SELECT
    o.UserId,
    o.FirstName,
    o.LastName,
    o.Email,
    o.Phone,
    o.Status,
    o.CreatedOn,
    o.CreatedBy,
    o.ModifiedOn,
    o.ModifiedBy
FROM 
    Users o
    INNER JOIN UserOrganisation uo1
        ON o.UserId = uo1.UserId
    INNER JOIN UserOrganisation uo2
        ON uo1.OrgId = uo2.OrgId
WHERE
    uo1.UserId = @SearchUserId
    AND uo2.UserId = @UserId

/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR

RETURN 0

/* error-handling */
ERROR:
    RETURN @error

/* End of procedure */
