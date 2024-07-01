CREATE PROCEDURE dbo.Configuration_List
(
	@Entity nvarchar(100) = NULL
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */

SELECT
	c.Entity,
	c.[Key],
	c.Value,
	c.Status,
	c.CreatedOn,
	c.CreatedBy,
	c.ModifiedOn,
	c.ModifiedBy
FROM 
	Configuration c
WHERE
    ((c.Entity = @Entity) OR (@Entity IS NULL))
ORDER BY
   c.Entity,
   c.[Key]

/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR

RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/
