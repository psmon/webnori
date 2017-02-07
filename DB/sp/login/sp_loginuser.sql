DROP PROCEDURE IF EXISTS sp_loginuser;
DELIMITER $$
CREATE PROCEDURE sp_loginuser (
  IN _socialType INT,
  IN _socialID VARCHAR(45),
  IN _pw VARCHAR(45)
  )
BEGIN  
  #Result Value  
  DECLARE AuthToken VARCHAR(45);
  DECLARE Result INT;
  DECLARE MemberID INT;
  DECLARE SocialType INT;
  DECLARE SocialID VARCHAR(45);
  DECLARE Nick VARCHAR(45);
  DECLARE PicUrl VARCHAR(45);
  DECLARE Email VARCHAR(45);
  DECLARE Createtime INT;
  
  #Tmp Value
  DECLARE _usercount INT;
  DECLARE _memberID INT;    
  DECLARE _currentTime INT;  
    
  
  Select UNIX_TIMESTAMP(current_timestamp) into _currentTime;
  #SET _currentTime =(SELECT current_timestamp());
  
  select COUNT(*),tblmember.memberID into _usercount,_memberID from tblmember where 
    tblmember.socialType = _socialType and 
    tblmember.socialID = _socialID and 
    tblmember.pw = _pw;
    
  SET AuthToken= CONCAT(_memberID , CONCAT("-",(SELECT uuid())) );
  SET Result = 0;
        
  if _usercount>0 then
  begin
    select tblmember.memberID,tblmember.socialType,tblmember.socialID,tblmember.nick,tblmember.picurl,tblmember.email,tblmember.createtime into 
        MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime  from tblmember where tblmember.memberID = _memberID;
        
    insert into tblloginstate (memberid,authtoken,lastlogintime) VALUES(_memberID,AuthToken,_currentTime) ON DUPLICATE KEY UPDATE authtoken=AuthToken,lastlogintime=_currentTime;
    update tblloginstate set lastlogintime=_currentTime where tblloginstate.memberid=_memberID;
    
  end;    
  else
    SET Result = -1;
  end if;
  
  if Result = 0 then
    select Result,AuthToken,MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime;    
  else
    select Result;
  end if;
    
  
END $$
DELIMITER ;

