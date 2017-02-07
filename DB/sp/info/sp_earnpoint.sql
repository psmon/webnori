DROP PROCEDURE IF EXISTS sp_earnpoint;
DELIMITER $$
CREATE PROCEDURE sp_earnpoint (
  IN _authtoken VARCHAR(45),
  IN userNO INT,
  IN monthData VARCHAR(45)
  )
BEGIN
  #Result Value
  DECLARE Result INT;
  DECLARE MemberID INT;
  DECLARE EarnPointTotal INT;  
  
  
  #Tmp Value
  DECLARE usercount INT; 
  DECLARE currentTime INT; 
  DECLARE starttime INT;
  DECLARE _createTime INT;
  
  set monthData = CONCAT(monthData ,' 00:00:00');
  
  Select UNIX_TIMESTAMP( monthData) into starttime; 
  #Select UNIX_TIMESTAMP(current_timestamp) into currentTime;  
  
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
    select sum(betAmount),joinTime into EarnPointTotal,_createTime FROM tblgamehistory where winnerID=MemberID and starttime<=joinTime;    
    set Result = 0;
  end;
  else
    set Result = -1;
  end if;
  
  if Result = 0 then
    
    if isnull(EarnPointTotal) then
      set EarnPointTotal = 0;
    end if;            
    
    select Result,EarnPointTotal,starttime,_createTime;
  else
    select Result;
  end if;
      
  
END $$
DELIMITER ;

