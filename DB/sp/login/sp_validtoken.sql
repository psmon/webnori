DROP PROCEDURE IF EXISTS sp_validtoken;
DELIMITER $$
CREATE PROCEDURE sp_validtoken (
  IN _authtoken VARCHAR(45),
  IN isUpdateLoginTime INT
  )
BEGIN
  #Result Value
  DECLARE Result INT;
  DECLARE MemberID INT;
  DECLARE SocialType INT;
  DECLARE SocialID VARCHAR(45);
  DECLARE Nick VARCHAR(45);
  DECLARE PicUrl VARCHAR(45);
  DECLARE Email VARCHAR(45);
  DECLARE Createtime INT;
  
  #Tmp Value
  DECLARE usercount INT;
  DECLARE _currentTime INT;  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION 
  BEGIN
    ROLLBACK;        
    set Result = -100;
    SELECT Result;
    SHOW WARNINGS; #DIAGNOSTICS
  END;
	
  START TRANSACTION;
  Select UNIX_TIMESTAMP(current_timestamp) into _currentTime;
  
    
  select COUNT(*),tblloginstate.memberid into usercount,MemberID from tblloginstate where tblloginstate.authtoken = _authtoken;  
  if usercount >0 then
  begin        
    select tblmember.memberID,tblmember.socialType,tblmember.socialID,tblmember.nick,tblmember.picurl,tblmember.email,tblmember.createtime into 
      MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime  from tblmember where tblmember.memberID = MemberID;
      	
    if(isUpdateLoginTime=1) then
    begin      
      update tblloginstate set tblloginstate.lastlogintime = _currentTime where tblloginstate.memberid = MemberID;
    end;
    end if;
    
    set Result = 0;
  end;
  else  #Not Exist User By AuthToken
    set Result = -1;    
  end if;
  
  if Result = 0 then
    select Result,MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime;
  else
    select Result;
  end if;
  
  COMMIT;  
      
  
END $$
DELIMITER ;

