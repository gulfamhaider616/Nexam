-- PublishQuestionByAdmin used to be a plain SELECT, so the admin "Publish" button never saved anything
-- (it returned "no rows affected"). It now sets IsPublish = 1. Safe to run more than once.
CREATE OR ALTER PROCEDURE [dbo].[PublishQuestionByAdmin]
    @QuestionID int
AS
BEGIN
    SET NOCOUNT OFF;
    UPDATE Forum SET IsPublish = 1 WHERE QuestionID = @QuestionID;
END
GO
