CREATE PROCEDURE [dbo].[spExceptionLog_Delete]
	@Id INT
AS
BEGIN
	DELETE FROM dbo.[ExceptionLog] WHERE Id = @Id;
END
