DROP DATABASE [FenXs-Accounts]
GO
CREATE DATABASE [FenXs-Accounts]
GO
USE [FenXs-Accounts]
GO
CREATE TABLE [Users]
(
  Id INT NOT NULL IDENTITY(1,1),
  Login VARCHAR(15) NOT NULL UNIQUE,
  Password VARCHAR(320) NOT NULL,
  Email VARCHAR(320) NOT NULL UNIQUE,
  Active BIT NOT NULL DEFAULT 0,
  Banned BIT NOT NULL DEFAULT 0,
  Admin BIT NOT NULL DEFAULT 0,
  FenXs_Stars INT NOT NULL DEFAULT 0,
  SignInDate DATETIME NOT NULL DEFAULT GETDATE(),
  LastLogIn DATETIME NOT NULL DEFAULT GETDATE()
    CONSTRAINT [PK_ACCOUNT] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)