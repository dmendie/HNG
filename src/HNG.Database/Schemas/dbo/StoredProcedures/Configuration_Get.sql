CREATE PROCEDURE dbo.Configuration_Get
(
	@Entity nvarchar(100),
	@Key nvarchar(500)
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
	c.Entity = @Entity
	AND c.[Key] = @Key

/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR


RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/
