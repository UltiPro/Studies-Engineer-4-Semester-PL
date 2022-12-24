DROP PROCEDURE GetUser
GO
Create procedure GetUser 
(    
   @login varchar(50),
   @password varchar(50)    
)    
as     
begin    
   Select * from UserProfile where UserName = @login AND Password = @password;
End