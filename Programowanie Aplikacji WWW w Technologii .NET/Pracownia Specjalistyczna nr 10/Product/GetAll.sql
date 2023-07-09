DROP PROCEDURE GetProducts
GO
Create procedure GetProducts
as
Begin
    Select *
    from Product
End