-- Original table schema
CREATE TABLE [Person] (
    [ID] integer PRIMARY KEY NOT NULL,
    [Person] varchar(50) NOT NULL,
    [TestTableID] integer NOT NULL,
    CONSTRAINT [FK_Person_TestTableID_TestTable_ID] FOREIGN KEY ([TestTableID]) REFERENCES [TestTable] ([ID]) ON UPDATE CASCADE ON DELETE CASCADE
);
