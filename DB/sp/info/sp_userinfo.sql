DROP PROCEDURE IF EXISTS sp_userinfo;
DELIMITER $$
CREATE PROCEDURE sp_userinfo (
  IN _authtoken VARCHAR(45),
  IN userNO INT
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
  
  DECLARE GamePoint INT;
  DECLARE CoinCount INT;
  DECLARE WinCount INT;
  DECLARE LoseCount INT;
  
  #Tmp Value
  DECLARE usercount INT;  
  
  IF(userNO > 0) THEN  
  BEGIN
    set usercount=1;
    set MemberID = userNO;
  END;
  ELSE
    select COUNT(*),tblloginstate.memberid into usercount,MemberID from tblloginstate where tblloginstate.authtoken = _authtoken;
  END IF;
  
  if usercount >0 then
  begin        
    select tblmember.memberID,tblmember.socialType,tblmember.socialID,tblmember.nick,tblmember.picurl,tblmember.email,tblmember.createtime 
    into MemberID,SocialType,SocialID,Nick,PicUrl,Email,Createtime  from tblmember where tblmember.memberID = MemberID;
    
    select tblbankinfo.gamePoint,tblbankinfo.coinCount,tblbankinfo.winCount,tblbankinfo.loseCount
      into GamePoint,CoinCount,WinCount,LoseCount FROM tblbankinfo where tblbankinfo.memberID = MemberID;
      
    set Result = 0;
  end;
  else
    set Result = -1;
  end if;
  
  if Result = 0 then
    select Result,Nick,PicUrl,Email,GamePoint,CoinCount,WinCount,LoseCount,MemberID;
  else
    select Result;
  end if;
      
  
END $$
DELIMITER ;

