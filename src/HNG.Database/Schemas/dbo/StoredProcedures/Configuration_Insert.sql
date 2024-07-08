CREATE PROCEDURE dbo.Configuration_Insert
(
	@Entity nvarchar(100),
	@Key nvarchar(500),
	@Value nvarchar(1000),
	@CreatedBy nvarchar(255)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int,
    @Status int = 1

/* Beginning of procedure */
BEGIN TRANSACTION

INSERT INTO Configuration 
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
	@CreatedBy,
	GETUTCDATE(),
	@CreatedBy
)

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
	@CreatedBy,
	GETUTCDATE(),
	@CreatedBy
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
