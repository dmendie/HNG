CREATE PROCEDURE dbo.Configuration_Update
(
	@Entity nvarchar(100),
	@Key nvarchar(500),
	@Value nvarchar(1000),
	@ModifiedBy nvarchar(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int,
    @Status int 

/* Beginning of procedure */
SELECT 
	@Status = Status
FROM Configuration
WHERE
    Entity = @Entity AND
    [Key] = @Key


BEGIN TRANSACTION


UPDATE Configuration SET
	Value = @Value,
	ModifiedOn = GETUTCDATE(),
	ModifiedBy = @ModifiedBy
WHERE
	Entity = @Entity	AND
	[Key] = @Key

/* error-handling */
SELECT @error = @@error
IF (@error <> 0) GOTO ERROR

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
VALUES
(
	@Entity,
	@Key,
	@Value,
	@Status,
	GETUTCDATE(),
	@ModifiedBy,
	GETUTCDATE(),
	@ModifiedBy
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
