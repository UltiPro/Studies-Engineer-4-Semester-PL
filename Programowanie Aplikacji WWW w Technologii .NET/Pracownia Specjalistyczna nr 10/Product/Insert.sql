DROP PROCEDURE AddProduct
GO
Create procedure AddProduct
(  
    @Name NCHAR(50),   
    @Money MONEY,
    @CId int
)  
as   
Begin   
    Insert into Product(name,price,Id_cat)   
    Values (@Name,@Money,@CID)   
End 