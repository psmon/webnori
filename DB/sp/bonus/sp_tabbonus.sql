DROP PROCEDURE IF EXISTS sp_tabbonus;
DELIMITER $$
CREATE PROCEDURE sp_tabbonus (
  IN playerAuthToken VARCHAR(45)
  )
BEGIN    
  #Result Value    
  DECLARE Result INT;
  
  #Tmp Value      
  DECLARE _creatorID INT;     
  
  DECLARE _gamePoint INT;    
  DECLARE _coinCount INT;    
  
  DECLARE _lastAdwardBonusTime INT; 
  DECLARE dayOffSet INT; 
  DECLARE currentTime INT;
  DECLARE BonusPoint INT default 0;  
  DECLARE BonusCoin INT default 0;
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN
    ROLLBACK;    
    set Result = -100;
    SELECT Result;    
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;  
  set Result = 0;    
  set dayOffSet = 60 * 60 * 24;
  select UNIX_TIMESTAMP(current_timestamp) into currentTime;  
       
  select tblloginstate.memberid into _creatorID from tblloginstate where tblloginstate.authtoken = playerAuthToken;    
  select gamePoint,coinCount,lastAdwardBonusTime into _gamePoint,_coinCount,_lastAdwardBonusTime from tblbankinfo where tblbankinfo.memberID = _creatorID;
    
  IF isnull(_creatorID) THEN
    set Result = -1;
    ROLLBACK;    
  END IF;
    
  
  if Result=0 then
    
    if _lastAdwardBonusTime < (currentTime - dayOffSet)   then
    begin
      set BonusPoint=500;
      set BonusCoin=0;      
      set _gamePoint = _gamePoint + BonusPoint; 
      set _coinCount = _coinCount + BonusCoin; 
      update tblbankinfo set gamePoint=_gamePoint,coinCount=_coinCount,lastAdwardBonusTime=currentTime where tblbankinfo.memberID = _creatorID;
      commit;
    end;
    end if;    
  begin
  end;
  end if;
    
  SELECT Result,Bonus;
  
END $$
DELIMITER ;

