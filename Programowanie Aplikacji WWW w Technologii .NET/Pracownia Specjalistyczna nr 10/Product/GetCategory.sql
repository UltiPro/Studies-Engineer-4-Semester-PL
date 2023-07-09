DROP PROCEDURE GetCategory
GO
Create procedure GetCategory
   (
   @CId int
)
as
begin
   Select *
   from Category
   where Id = @CId
End