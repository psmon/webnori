DROP PROCEDURE IF EXISTS sp_CreateQHoldem;
DELIMITER $$
CREATE PROCEDURE sp_CreateQHoldem (
  IN mainType INT,
  IN gameTitle VARCHAR(50),
  IN gameData VARCHAR(20),
  IN gameCount INT,
  IN gameMoneyAmount INT,
  IN creatorAuthToken VARCHAR(45)
  )
BEGIN    
  #Result Value    
  DECLARE Result INT;
  
  #Tmp Value    
  DECLARE _createtime INT;
  DECLARE _curGameIdx INT;  
  DECLARE _creatorID INT;
  DECLARE _creatorNick VARCHAR(45);
  DECLARE _creatorAvatar VARCHAR(45);
  
  DECLARE _gamePoint INT;
  DECLARE _lockPoint INT;
  DECLARE _coinCount INT;
  DECLARE _publishCnt INT;
  DECLARE _gameID INT;
  
  
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION BEGIN
    ROLLBACK;    
    set Result = -100;
    SELECT Result;    
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;  
  set Result = 0;    
  
  Select UNIX_TIMESTAMP(current_timestamp) into _createtime;      
  select tblloginstate.memberid into _creatorID from tblloginstate where tblloginstate.authtoken = creatorAuthToken;  
  select tblbankinfo.gamePoint,tblbankinfo.coinCount into _gamePoint,_coinCount from tblbankinfo where tblbankinfo.memberID = _creatorID;      
  select Count(*) into _publishCnt from tblQHoldemList where creatorID = _creatorID and tblQHoldemList.gameState = 0 and tblQHoldemList.mainType=0;  
    
  IF _creatorID = NULL THEN
    set Result = -1;
    ROLLBACK;    
  END IF;
  
  if mainType=0 then
  begin
    IF _gamePoint < gameMoneyAmount*gameCount THEN
    set Result = -2;
    ROLLBACK;    
    END IF;
  
    IF 3 < _publishCnt Then
      set Result = -3;
      ROLLBACK;    
    END IF;    
        
  end;
  end if;
  
        
  IF Result=0 THEN
  BEGIN
    set _gamePoint = _gamePoint - gameMoneyAmount*gameCount;
    set _lockPoint = gameMoneyAmount*gameCount;
    
    if mainType=0 then
      update tblbankinfo set gamePoint = _gamePoint where memberID = _creatorID;  
    end if;
    
    select nick,picurl into _creatorNick,_creatorAvatar from tblmember where memberID = _creatorID;    
  
    insert into tblQHoldemList( 
      gameID,mainType,gameState,gameTitle,creatorID,creatorNick,gameData,gameCount,curGameIdx,gameMoneyAmount,createTime,creatorAvatar,lockPoint
    )
    values(
      NULL,mainType,0,gameTitle,_creatorID,_creatorNick,gameData,gameCount,0,gameMoneyAmount,_createtime,_creatorAvatar,_lockPoint    
    );    
    COMMIT;    
  END;
  END IF;
    
  SELECT LAST_INSERT_ID() into _gameID;    
  SELECT Result,_gameID;
  
END $$
DELIMITER ;

