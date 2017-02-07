DROP PROCEDURE IF EXISTS sp_EdtIndianHoldem;
DELIMITER $$
CREATE PROCEDURE sp_EdtIndianHoldem (  
  IN gameID INT,
  IN gameAction INT,
  IN creatorAuthToken VARCHAR(45)
  )
BEGIN    
  #Result Value    
  DECLARE Result INT;
  
  #Tmp Value      
  DECLARE _creatorID INT;
  
  DECLARE _gamePoint INT;
  DECLARE _lockPoint INT;
  DECLARE _fixgamePoint INT;
  DECLARE _fixlockPoint INT;
  DECLARE _fixgameMoneyAmount INT;
  DECLARE _gameMoneyAmount INT;
  DECLARE _gameCount INT;
  DECLARE _publishCnt INT;
  DECLARE _isFixAmount BOOL DEFAULT FALSE;
  DECLARE _mainType INT;
    
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN
    ROLLBACK;    
    set Result = -100;
    SELECT Result;    
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;  
  set Result = 0;    
       
  select tblloginstate.memberid into _creatorID from tblloginstate where tblloginstate.authtoken = creatorAuthToken;  
  select tblbankinfo.gamePoint into _gamePoint from tblbankinfo where tblbankinfo.memberID = _creatorID;      
  select Count(*),lockPoint,gameMoneyAmount,gameCount,mainType into _publishCnt,_lockPoint,_gameMoneyAmount,_gameCount,_mainType from tblIndianHoldemList where creatorID = _creatorID and gameState = -1;  
    
  IF _creatorID = NULL THEN
    set Result = -1;
    ROLLBACK;    
  END IF;
  
        
  IF Result=0 THEN
  BEGIN
    set _fixgamePoint = _gamePoint;
    set _fixlockPoint = _lockPoint;
    set _fixgameMoneyAmount = _gameMoneyAmount;
    
    if gameAction=0 then
    begin
      set _fixgameMoneyAmount = _gameMoneyAmount / 2;
      set _fixgamePoint = _gamePoint + _gameCount*_fixgameMoneyAmount;
      set _fixlockPoint = _lockPoint - _gameCount*_fixgameMoneyAmount;
      set _isFixAmount=true;
    end;
    end if;
    
    if gameAction=2 then
    begin
      set _fixgameMoneyAmount = _gameMoneyAmount * 2;
      set _fixgamePoint = _gamePoint - _gameCount*_gameMoneyAmount;
      set _fixlockPoint = _lockPoint + _gameCount*_gameMoneyAmount;
      if _fixgamePoint>-1 then
        set _isFixAmount = true;
      else
        set _fixgameMoneyAmount = _gameMoneyAmount;
      end if;
    end;
    end if;
    
    if _isFixAmount=true then
    begin
      if _mainType=0 then
        update tblbankinfo set gamePoint = _fixgamePoint where memberID = _creatorID;
      end if;
        
      update tblindianholdemlist set gameMoneyAmount=_fixgameMoneyAmount,lockPoint=_fixlockPoint,gameState=0 where tblindianholdemlist.gameID=gameID;
    end;
    else
      update tblindianholdemlist set gameState=0 where tblindianholdemlist.gameID=gameID;
    end if;    
        
    COMMIT;    
  END;
  END IF;
        
  SELECT Result,_fixgameMoneyAmount;
  
END $$
DELIMITER ;

