DROP PROCEDURE GetProduct
GO
Create procedure GetProduct
   (
   @PId int
)
as
begin
   Select *
   from Product
   where Id = @PId
End