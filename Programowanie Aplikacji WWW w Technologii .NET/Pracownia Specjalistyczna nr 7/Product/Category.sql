CREATE TABLE [dbo].[Category]
(
  [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [shortName] NCHAR(5) NOT NULL,
  [longName] NCHAR(50) NOT NULL,
)

INSERT INTO Category
  (shortName,longName)
VALUES('SPZ', 'Spozywcze');
INSERT INTO Category
  (shortName,longName)
VALUES('SPR', 'Sportowe');
INSERT INTO Category
  (shortName,longName)
VALUES('PRZ', 'Przemyslowe');
INSERT INTO Category
  (shortName,longName)
VALUES('ROL', 'Rolnicze');