DROP PROCEDURE IF EXISTS sp_RunQHoldem;
DELIMITER $$
CREATE PROCEDURE sp_RunQHoldem (
  IN gameID INT,
  IN joinerGameData INT,  
  IN joinerMemo VARCHAR(50),
  IN joinerAuthToken VARCHAR(45),
  IN commCardData VARCHAR(20),  
  IN whoWinner INT, #0:Crator , 1:Joiner
  IN creatorHandStr VARCHAR(45),
  IN joinerHandStr VARCHAR(45)
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
  
  DECLARE _joinerID INT;
  DECLARE _joinerNick VARCHAR(20);
  DECLARE _joinerAvatar VARCHAR(45);
  DECLARE _joinerGamePoint INT;      
  
  DECLARE _creatorID INT;
  DECLARE _cratorGameData VARCHAR(45);    
  DECLARE _creatorGamePoint INT;
  DECLARE _creatorLockPoint INT;
  
  DECLARE _joinCnt INT;
  DECLARE _coinCount INT;
  
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
    from tblQHoldemList where tblQHoldemList.gameID = gameID;
  
  select memberid 
    into _joinerID from tblloginstate where tblloginstate.authtoken = joinerAuthToken;
  
  select gamePoint ,coinCount
    into _joinerGamePoint,_coinCount from tblbankinfo where tblbankinfo.memberID = _joinerID;
  
  set _coinCount = _coinCount+1;  
    
  select nick,picurl 
    into _joinerNick,_joinerAvatar 
    from tblmember 
    where tblmember.memberID = _joinerID;
  
    
  select Count(*) 
    into _joinCnt 
    from tblQHoldemDetail 
    where tblQHoldemDetail.gameID = gameID and tblQHoldemDetail.joinerID = _joinerID;        
      
  #Error Check
  if _subGameID=null or _joinerID=null then  
    SET Result = -1;  #not exist game or user  
  end if;
  
  if _gameCount = _subGameID then
    SET Result = -2;  #fullGame
  end if;
  
  if _joinerGamePoint < _gameMoneyAmount then
    SET Result = -3;  #not enoght money
  end if;
  
  if _cur_gameState = 1 then
    SET Result = -4;  #completed game
  end if;
  
  if _creatorID = _joinerID then
    SET Result = -5;  #same Game
  end if;
  
  if _joinCnt > 0 then
    SET Result = -6;
  end if;
      
  if Result = 0 then
  begin
    set _nextGameID = _subGameID+1;
    update tblQHoldemList SET curGameIdx = _nextGameID where tblQHoldemList.gameID = gameID;
    
    IF whoWinner=1 THEN
    BEGIN
      set _winnerID = _creatorID;
      set _creatorLockPoint =_creatorLockPoint + _gameMoneyAmount;
     
      update tblQHoldemList 
        set lockPoint=_creatorLockPoint 
        where tblQHoldemList.gameID = gameID;
      
      set _joinerGamePoint = _joinerGamePoint - _gameMoneyAmount;
      
      update tblbankinfo 
        set gamePoint = _joinerGamePoint, loseCount=loseCount+1
        where tblbankinfo.memberID = _joinerID;              
      
      update tblbankinfo 
        set winCount=winCount+1 
        where tblbankinfo.memberID = _creatorID;
        
      insert into tblgamehistory(gameType,gameNo,gameSubNo,winnerID,loseID,betAmount,joinTime)
        values(2,gameID,_subGameID,_winnerID,_joinerID,_gameMoneyAmount,currentTime);
        
      
    END;
    ELSEIF  whoWinner=2 THEN
    BEGIN
      set _winnerID = _joinerID;      
      set _creatorLockPoint =_creatorLockPoint - _gameMoneyAmount;
      
      update tblQHoldemList 
        SET lockPoint=_creatorLockPoint 
        where tblQHoldemList.gameID = gameID;
        
      set _joinerGamePoint = _joinerGamePoint + _gameMoneyAmount;
      update tblbankinfo 
        SET gamePoint = _joinerGamePoint,winCount = winCount+1 
        where tblbankinfo.memberID = _joinerID;              
      
      update tblbankinfo 
        SET loseCount=loseCount+1 
        where tblbankinfo.memberID = _creatorID;
        
      insert into tblgamehistory(gameType,gameNo,gameSubNo,winnerID,loseID,betAmount,joinTime)
        values(2,gameID,_subGameID,_winnerID,_creatorID,_gameMoneyAmount,currentTime);
        
    END;
    ELSE
    BEGIN
      #draw game...
      SET _winnerID = 0;
      SET _gameMoneyAmount = 0;
      update tblbankinfo 
        SET coinCount=_coinCount 
        where tblbankinfo.memberID = _joinerID;
    END;
    END IF; 
    
    INSERT INTO tblQHoldemDetail(
      gameID,subGameID,joinerID,joinerNick,joinerAvatar,joinerMemo,joinerGameData,commCardData,earnPoint,winnerID,joinTime,creatorHandStr,joinerHandStr
    )
    VALUES(
      gameID,_nextGameID,_joinerID,_joinerNick,_joinerAvatar,joinerMemo,joinerGameData,commCardData,_gameMoneyAmount,_winnerID,currentTime,creatorHandStr,joinerHandStr
    );    
  end;
  end if;
  
  
  #After Update ( complete Game )
  if _nextGameID = _gameCount and Result=0 then
  begin    
    update tblQHoldemList SET gameState = 1 where tblQHoldemList.gameID = gameID;
  end;
  end if;
  
  COMMIT;
    
  
  SELECT Result;  
        
END $$
DELIMITER ;

