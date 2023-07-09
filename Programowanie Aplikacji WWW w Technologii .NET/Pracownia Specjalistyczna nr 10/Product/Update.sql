DROP PROCEDURE UpdateProduct
GO
Create procedure UpdateProduct
	(
	@PId INTEGER,
	@Name NCHAR(50),
	@Money MONEY,
	@CId int
)
as
begin
	Update Product     
	set Name=@Name,price=@Money,Id_cat=@CId 
	where Id=@PId
End