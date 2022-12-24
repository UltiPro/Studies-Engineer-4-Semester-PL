CREATE TABLE [dbo].[Product]
(
  [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
  [name] NCHAR(50) NOT NULL,
  [price] MONEY NOT NULL,
  [Id_cat] INT NOT NULL CONSTRAINT FK_CP REFERENCES Category(Id)
);

INSERT INTO Product(name,price,Id_cat) VALUES('Narty',156,2);
INSERT INTO Product(name,price,Id_cat) VALUES('Piłka nożna',100,2);
INSERT INTO Product(name,price,Id_cat) VALUES('Rakieta',212,2);
INSERT INTO Product(name,price,Id_cat) VALUES('Lody',99.99,1);
INSERT INTO Product(name,price,Id_cat) VALUES('Ruda Miedzi',55.5,3);
INSERT INTO Product(name,price,Id_cat) VALUES('Zboże',1000,4);