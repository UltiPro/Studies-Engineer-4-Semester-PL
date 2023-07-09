DROP PROCEDURE GetCategories
GO
Create procedure GetCategories
as
Begin
    Select *
    from Category
End