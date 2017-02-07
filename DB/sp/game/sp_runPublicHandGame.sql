DROP PROCEDURE IF EXISTS sp_runPublicHandGame;
DELIMITER $$
CREATE PROCEDURE sp_runPublicHandGame (
  IN gameID INT,
  IN gameFlag_right1 VARCHAR(45),
  IN memberNick_right VARCHAR(45),
  IN gameMemo_right VARCHAR(200),
  IN whoWinner INT #0:left , 1:right
  )
BEGIN  
  DECLARE Result INT;
  DECLARE usercount INT;  
  DECLARE subGameID INT;
  DECLARE nextGameID INT;
  DECLARE gameCount INT;    
  DECLARE winnerID INT;
  DECLARE earnPoint INT;    
  DECLARE memberID_right INT default -1;
  DECLARE MemberID INT default -1;    
  DECLARE gameType INT;
  DECLARE gameFlag_left1 VARCHAR(45);
  DECLARE MemberID_left INT;      
  DECLARE lockPoint_left INT;    
  DECLARE cur_gameState INT;  
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
  
  select tblHandGameList.curGameIdx,tblHandGameList.gameMoneyAmount,tblHandGameList.gameCount,tblHandGameList.gameType,
          tblHandGameList.gameFlag_left1,tblHandGameList.MemberID_left,tblHandGameList.lockPoint,tblHandGameList.gameState
    into subGameID,earnPoint,gameCount,gameType,gameFlag_left1,MemberID_left,lockPoint_left,cur_gameState 
    from tblHandGameList where tblHandGameList.gameID = gameID and tblHandGameList.mainType=1;
                
      
  #Error Check
  if subGameID=null or MemberID=null then  
    SET Result = -1;  #not exist game or user  
  end if;
  
  if gameCount = subGameID then
    SET Result = -2;  #fullGame
  end if;
    
  
  if cur_gameState = 1 then
    SET Result = -4;  #completed game
  end if;
    
      
  if Result = 0 then
  begin
    set nextGameID = subGameID+1;
    update tblHandGameList SET curGameIdx = nextGameID where tblHandGameList.gameID = gameID;
    
    IF whoWinner=1 THEN
    BEGIN
      SET winnerID = MemberID_left;      
    END;
    ELSEIF  whoWinner=2 THEN
    BEGIN
      SET winnerID = MemberID;
    END;
    ELSE
    BEGIN
      #draw game...
      SET winnerID = 0;
      SET earnPoint = 0;
    END;
    END IF; 
    
    INSERT INTO tblHandGameDetail(
      gameID,subGameID,gameFlag_right1,memberID_right,memberNick_right,
      gameMemo_right,winnerID,earnPoint,joinTime
    )
    VALUES(
      gameID,subGameID,gameFlag_right1,MemberID,memberNick_right,
      gameMemo_right,winnerID,earnPoint,currentTime
    );
  end;
  end if;
  
  
  #After Update ( complete Game )
  if nextGameID = gameCount and Result=0 then
  begin            
    update tblHandGameList SET gameState = 1 where tblHandGameList.gameID = gameID;
  end;
  end if;
  
  SELECT Result;
  COMMIT;
        
END $$
DELIMITER ;

