CREATE PROCEDURE [dbo].[spExceptionLog_Create]
	@AccountId NCHAR(16) NULL,
	@Exception NVARCHAR(64),
	@Id INT OUT
AS
SET NOCOUNT ON;
BEGIN
	INSERT INTO dbo.[ExceptionLog](
	[AccountId],
	[Exception])

	VALUES(
	@AccountId,
	@Exception)

	SET @Id = SCOPE_IDENTITY();
END
