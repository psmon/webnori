use webnori;
DROP PROCEDURE IF EXISTS sp_RunPublicIndianHoldem;
DELIMITER $$
CREATE PROCEDURE sp_RunPublicIndianHoldem (
  IN gameID INT,
  IN joinerGameData VARCHAR(45),  
  IN joinerMemo VARCHAR(50),  
  IN gameAction INT,
  IN whoWinner INT, #0:Crator , 1:Joiner
  IN creatorHandStr VARCHAR(45),
  IN joinerHandStr VARCHAR(45),
  IN joinerNick VARCHAR(20),
  IN joinerAvatar VARCHAR(45)
  )
BEGIN  
  DECLARE Result INT;
  DECLARE _usercount INT;  
  DECLARE _subGameID INT;
  DECLARE _nextGameID INT;
  DECLARE _gameCount INT;      
  DECLARE _winnerID INT;
  DECLARE _gameMoneyAmount INT;          
  DECLARE _cur_gameState INT;
  
  DECLARE _joinerID INT default -1;    
  
  DECLARE _creatorID INT;
  DECLARE _cratorGameData VARCHAR(45);
  DECLARE _creatorGamePoint INT;
  DECLARE _creatorLockPoint INT;
    
  DECLARE currentTime INT;
  
  
  DECLARE EXIT HANDLER FOR SQLEXCEPTION 
  BEGIN
    ROLLBACK;        
    set Result = -100;
    SELECT Result;
    SHOW WARNINGS; #DIAGNOSTICS    
  END;
  
  START TRANSACTION;
  
  Select UNIX_TIMESTAMP(current_timestamp) into currentTime;  
  
  SET Result = 0;
  
  select curGameIdx,gameMoneyAmount,gameCount,gameData,creatorID,lockPoint,gameState
    into _subGameID,_gameMoneyAmount,_gameCount,_cratorGameData,_creatorID,_creatorLockPoint,_cur_gameState
    from tblIndianHoldemList where tblIndianHoldemList.gameID = gameID and tblIndianHoldemList.mainType=1;    
        
  if gameAction = 0 then
    set _gameMoneyAmount = _gameMoneyAmount /2;
  end if;
  
  if gameAction = 1 then
    set _gameMoneyAmount = _gameMoneyAmount;
  end if;
  
  if gameAction = 2 then
    set _gameMoneyAmount = _gameMoneyAmount;
  end if;
    
      
  #Error Check
  if _subGameID =null then  
    SET Result = -1;  #not exist game or user  
  end if;
  
  if _gameCount = _subGameID then
    SET Result = -2;  #fullGame
  end if;    
  
  if _cur_gameState = 1 then
    SET Result = -4;  #completed game
  end if;
  
  if _creatorID = _joinerID then
    SET Result = -5;  #same Game
  end if;    
      
  if Result = 0 then
  begin
    set _nextGameID = _subGameID+1;
    update tblIndianHoldemList SET curGameIdx = _nextGameID where tblIndianHoldemList.gameID = gameID;
    
    IF whoWinner=1 THEN
    BEGIN
      set _winnerID = _creatorID;
      set _creatorLockPoint =_creatorLockPoint + _gameMoneyAmount;
     
      update tblIndianHoldemList 
        set lockPoint=_creatorLockPoint 
        where tblIndianHoldemList.gameID = gameID;                  
      
    END;
    ELSEIF  whoWinner=2 THEN
    BEGIN
      set _winnerID = _joinerID;      
      set _creatorLockPoint =_creatorLockPoint - _gameMoneyAmount;
      
      update tblIndianHoldemList 
        SET lockPoint=_creatorLockPoint 
        where tblIndianHoldemList.gameID = gameID;
        
    END;
    ELSE
    BEGIN
      #draw game...
      SET _winnerID = 0;
      SET _gameMoneyAmount = 0;      
    END;
    END IF; 
    
    INSERT INTO tblIndianHoldemDetail(
      gameID,subGameID,joinerID,joinerNick,joinerAvatar,joinerMemo,joinerGameData,earnPoint,winnerID,joinTime,creatorHandStr,joinerHandStr,action
    )
    VALUES(
      gameID,_nextGameID,_joinerID,joinerNick,joinerAvatar,joinerMemo,joinerGameData,_gameMoneyAmount,_winnerID,currentTime,creatorHandStr,joinerHandStr,gameAction
    );    
  end;
  end if;
  
  
  #After Update ( complete Game )
  if _nextGameID = _gameCount and Result=0 then
  begin    
    update tblIndianHoldemList SET gameState = 1 where tblIndianHoldemList.gameID = gameID;
  end;
  end if;
  
  COMMIT;
    
  
  SELECT Result;  
        
END $$
DELIMITER ;

