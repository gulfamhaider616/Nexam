-- Subject pages and per-subject counts filter Questions by SubjectID; without this every subject page scans the whole table
-- (fine at 25k questions, slow at a million). Safe to run more than once.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Questions_SubjectID' AND object_id = OBJECT_ID('dbo.Questions'))
    CREATE INDEX IX_Questions_SubjectID ON dbo.Questions (SubjectID);
GO
