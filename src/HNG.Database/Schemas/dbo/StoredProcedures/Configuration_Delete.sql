CREATE PROCEDURE dbo.Configuration_Delete
(
	@Entity nvarchar(100),
	@Key nvarchar(500),
    @ModifiedBy nvarchar(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */
BEGIN TRANSACTION

INSERT INTO ConfigurationHistory 
(
	Entity,
	[Key],
	Value,
	Status,
	CreatedOn,
	CreatedBy,
	ModifiedOn,
	ModifiedBy
)
SELECT 
	Entity,
	[Key],
	Value,
	0,
	GETUTCDATE(),
	@ModifiedBy,
	GETUTCDATE(),
	@ModifiedBy
FROM Configuration
WHERE
    Entity = @Entity AND
    [Key] = @Key

/* error-handling */
SELECT @error = @@error
IF (@error <> 0) GOTO ERROR

DELETE Configuration
WHERE
	Entity = @Entity	AND
	[Key] = @Key

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
