DROP PROCEDURE DeleteProduct
GO
Create procedure DeleteProduct 
(    
   @PId int    
)    
as     
begin    
   Delete from Product where Id = @PId  
End   