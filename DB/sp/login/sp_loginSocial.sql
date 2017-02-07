DROP PROCEDURE IF EXISTS sp_loginSocial;
DELIMITER $$
CREATE PROCEDURE sp_loginSocial (
  IN _socialType INT,
  IN _socialID VARCHAR(45),
  IN _nick VARCHAR(45),
  IN _picurl VARCHAR(45),
  IN _email VARCHAR(45)  
  )
BEGIN
  #Result Value    
  DECLARE Result INT DEFAULT 0;    
  DECLARE AuthToken VARCHAR(45);
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
  DECLARE _memberID INT;
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN
    ROLLBACK;    
    set Result = -100;
    SELECT Result;    
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
      
  Select UNIX_TIMESTAMP(current_timestamp) into _currentTime;
  select COUNT(*),tblmember.memberID into usercount,_memberID from tblmember where tblmember.socialType = _socialType and tblmember.socialID = _socialID;    
  
  #Auth RegisTer
  if(usercount=0) then
  begin
    insert into tblmember (
       memberID
      ,socialType
      ,socialID
      ,nick
      ,picurl
      ,email
      ,pw
      ,createtime
    ) VALUES (
       NULL -- memberID - IN int(11)
      ,_socialType -- socialType - IN int(11)
      ,_socialID -- socialID - IN varchar(45)
      ,_nick -- nick - IN varchar(45)
      ,_picurl  -- picurl - IN varchar(45)
      ,_email  -- email - IN varchar(45)
      ,"empty"  -- pw - IN varchar(45)
      ,_currentTime
    );    
    SELECT LAST_INSERT_ID() into _memberID;
  end;
  end if;
  
  SET AuthToken= CONCAT(_memberID , CONCAT("-",(SELECT uuid())) );
      
  select tblmember.memberID,tblmember.socialType,tblmember.socialID,tblmember.nick,tblmember.picurl,tblmember.email,tblmember.createtime into 
        MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime  from tblmember where tblmember.memberID = _memberID;        
        
  insert into tblloginstate (memberid,authtoken,lastlogintime) VALUES(_memberID,AuthToken,_currentTime) ON DUPLICATE KEY UPDATE authtoken=AuthToken,lastlogintime=_currentTime;
  update tblloginstate set lastlogintime=_currentTime where tblloginstate.memberid=_memberID;
      
  if Result = 0 then
    select Result,AuthToken,MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime;
    commit;
  else
    select Result;
  end if;
      
  
END $$
DELIMITER ;

